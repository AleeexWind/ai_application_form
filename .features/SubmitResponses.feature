Feature: Submit responses
    As a user I want to submit responses for provided questions of the questionnaire

    Scenario: Submit responses for provided questions
        Given I am logged in as a user
        And I am on questionnaire page (see GetQuestions.feature file)
        When I fill answers for the question
        And I push the button "Submit"
        Then I should see the message "Success"
        And The responses saves to the database