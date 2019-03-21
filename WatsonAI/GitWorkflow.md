# Git Workflow

The main branch is "ai", this should always compile and should be the latest
functioning version of the AI features.

Each new feature should be developed in a branch. These branches should
approximately correspond to a task on the Trello, or to an issue in the Git.
When the work is done, create a merge request in the Git and wait for it to be
reviewed. We can all review each other's commits and this way we can learn C#
from each other as we're going along.

When developing your own branch, make sure to merge from ai if there are any
changing in it, to make merging easier in the future.

Commit messages should be informative and ideally use proper English, at least
starting with a capital letter.
