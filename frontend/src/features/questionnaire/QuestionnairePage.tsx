import { FormEvent, useEffect, useState } from 'react';
import { useAuth } from '../auth/AuthContext';
import { fetchQuestions, Question, submitResponses } from './questionnaireService';

export default function QuestionnairePage() {
  const { signOut } = useAuth();
  const [questions, setQuestions] = useState<Question[]>([]);
  const [responses, setResponses] = useState<Record<number, string>>({});
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    async function loadQuestions() {
      try {
        const data = await fetchQuestions();
        setQuestions(data);
      } catch {
        setError('Unable to load questionnaire questions.');
      } finally {
        setIsLoading(false);
      }
    }

    loadQuestions();
  }, []);

  function handleResponseChange(position: number, value: string) {
    setResponses((current) => ({ ...current, [position]: value }));
    setSuccessMessage(null);
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError(null);
    setSuccessMessage(null);
    setIsSubmitting(true);

    try {
      const payload = questions.map((question) => ({
        position: question.position,
        answer: responses[question.position] ?? ''
      }));

      await submitResponses(payload);
      setSuccessMessage('Success');
    } catch {
      setError('Unable to submit responses.');
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <main className="page">
      <section className="card">
        <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <div>
            <h1>Questionnaire</h1>
            <p>Answer each question below.</p>
          </div>
          <button type="button" onClick={signOut}>
            Sign out
          </button>
        </header>

        {isLoading && <p>Loading questions...</p>}
        {error && <p className="error">{error}</p>}
        {successMessage && <p className="success">{successMessage}</p>}

        {!isLoading && !error && (
          <form onSubmit={handleSubmit}>
            {questions.map((question) => (
              <div key={question.position} className="question-item">
                <label htmlFor={`question-${question.position}`}>
                  {question.position}. {question.text}
                </label>
                <input
                  id={`question-${question.position}`}
                  type="text"
                  value={responses[question.position] ?? ''}
                  onChange={(event) => handleResponseChange(question.position, event.target.value)}
                  placeholder="Your response"
                />
              </div>
            ))}
            <button type="submit" disabled={isSubmitting}>
              {isSubmitting ? 'Submitting...' : 'Submit'}
            </button>
          </form>
        )}
      </section>
    </main>
  );
}
