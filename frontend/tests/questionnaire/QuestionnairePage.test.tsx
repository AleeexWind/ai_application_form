import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MemoryRouter } from 'react-router-dom';
import { describe, expect, it, vi, beforeEach } from 'vitest';
import QuestionnairePage from '../src/features/questionnaire/QuestionnairePage';
import { AuthProvider } from '../src/features/auth/AuthContext';

vi.mock('../src/features/questionnaire/questionnaireService', () => ({
  fetchQuestions: vi.fn(),
  submitResponses: vi.fn()
}));

import { fetchQuestions, submitResponses } from '../src/features/questionnaire/questionnaireService';

const mockedFetchQuestions = vi.mocked(fetchQuestions);
const mockedSubmitResponses = vi.mocked(submitResponses);

function renderQuestionnairePage() {
  return render(
    <AuthProvider>
      <MemoryRouter>
        <QuestionnairePage />
      </MemoryRouter>
    </AuthProvider>
  );
}

describe('QuestionnairePage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockedFetchQuestions.mockResolvedValue([
      { position: 1, text: 'What is your name?' },
      { position: 2, text: 'How old are you?' },
      { position: 3, text: 'What is your job?' }
    ]);
    mockedSubmitResponses.mockResolvedValue();
  });

  it('shows questions with response fields', async () => {
    renderQuestionnairePage();

    await waitFor(() => {
      expect(screen.getByLabelText(/What is your name\?/)).toBeInTheDocument();
      expect(screen.getByLabelText(/How old are you\?/)).toBeInTheDocument();
      expect(screen.getByLabelText(/What is your job\?/)).toBeInTheDocument();
    });
  });

  it('shows success message after submitting responses', async () => {
    const user = userEvent.setup();
    renderQuestionnairePage();

    await waitFor(() => {
      expect(screen.getByLabelText(/What is your name\?/)).toBeInTheDocument();
    });

    await user.type(screen.getByLabelText(/What is your name\?/), 'John Doe');
    await user.type(screen.getByLabelText(/How old are you\?/), '30');
    await user.type(screen.getByLabelText(/What is your job\?/), 'Developer');
    await user.click(screen.getByRole('button', { name: 'Submit' }));

    await waitFor(() => {
      expect(screen.getByText('Success')).toBeInTheDocument();
    });

    expect(mockedSubmitResponses).toHaveBeenCalledWith([
      { position: 1, answer: 'John Doe' },
      { position: 2, answer: '30' },
      { position: 3, answer: 'Developer' }
    ]);
  });
});
