﻿using Microsoft.TeamsAI.AI.Planner;
using Microsoft.TeamsAI.AI.Prompt;
using Microsoft.TeamsAI.State;
using Microsoft.Bot.Builder;

namespace Microsoft.TeamsAI.AI.Moderator
{
    /// <summary>
    /// A moderator is responsible for reviewing and approving AI prompts and plans.
    /// </summary>
    /// <typeparam name="TState">Type of the applications turn state.</typeparam>
    public interface IModerator<TState> where TState : ITurnState<StateBase, StateBase, TempState>
    {
        /// <summary>
        /// Reviews an incoming utterance and generated prompt before it's sent to the planner.
        /// </summary>
        /// <remarks>
        /// The moderator can review the incoming utterance for things like prompt injection attacks
        /// or the leakage of sensitive information. The moderator can also review the generated prompt
        /// to ensure it's appropriate for the current conversation.
        ///
        /// To approve a prompt, simply return null. Returning a new plan bypasses the planner and
        /// redirects to a new set of actions. Typically the moderator will return a new plan with a
        /// single DO command that calls <see cref="AIConstants.FlaggedInputActionName"/> to flag the input for review.
        ///
        /// The moderator can pass any entities that make sense to the redirected action.
        /// </remarks>
        /// <param name="turnContext">Context for the current turn of conversation.</param>
        /// <param name="turnState">Application state for the current turn of conversation.</param>
        /// <param name="prompt">Generated prompt to be reviewed.</param>
        /// <returns>A null value to approve the prompt or a new plan to redirect to if not approved.</returns>
        Task<Plan?> ReviewPrompt(ITurnContext turnContext, TState turnState, PromptTemplate prompt);

        /// <summary>
        /// Reviews a plan generated by the planner before its executed.
        /// </summary>
        /// <remarks>
        /// The moderator can review the plan to ensure it's appropriate for the current conversation.
        ///
        /// To approve a plan simply return the plan that was passed in. A new plan can be returned to
        /// redirect to a new set of actions. Typically the moderator will return a new plan with a
        /// single DO command that calls <see cref="AIConstants.FlaggedOutputActionName"/> to flag the output for review.
        ///
        /// The moderator can pass any entities that make sense to the redirected action.
        /// </remarks>
        /// <param name="turnContext">Context for the current turn of conversation.</param>
        /// <param name="turnState">Application state for the current turn of conversation.</param>
        /// <param name="plan">Plan generated by the planner.</param>
        /// <returns>The plan to execute. Either the current plan passed in for review or a new plan.</returns>
        Task<Plan> ReviewPlan(ITurnContext turnContext, TState turnState, Plan plan);
    }
}