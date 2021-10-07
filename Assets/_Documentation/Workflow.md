# Workflow

This document will take you through the whole workflow to fix an issue in the repository and create a PR!

## Key terminology

Repo: The git repository this project is hosted in. You push and pull the repo to sync changes to the project across
computers.

Local: Your local copy of the repo.

Remote: The shared remote copy of the repo on github. You push and pull to this remote.

Issue: An open request to develop something. Issues aren't necessarily bugs, but problems that need to be addressed.

Branch: The best way to understand the branch is to think of the entire repository as a tree. We have the master branch,
which is like the tree's trunk, and then we have child branches that extend out of certain commits. Whenever we want to
develop a new feature, we make a branch, add commits to it, and then merge it back into the master branch. This way,
multiple people can work on features at the same time and we can have a 'stable' master branch that holds our current
progress.

PR: Pull request. This is a request to merge a branch back into master.

## Making an issue

If you've identified something in the game that you'd like to see or change, please make an issue on it on Github. This
is a straightforward process but here are some helpful tips:

- Please be descriptive with your issue. Try and think about how you would go about implementing it and add that in the
  description.
- If you think you can break the issue up into smaller issues, do it. The smaller the issue, the better.
- Add relevant labels.

## Fixing an issue

Steps to fix an issue:

1. Assign yourself to an issue.
	- The most important thing to consider when deciding on an issue is whether you think you can actually complete it.
	  It's completely normal to not know exactly how to implement something when starting the issue out, but you should
	  have a general idea of how to go about fixing the issue. If you're unsure what's a good issue, ask on the discord.
	- The issue's priority is the next most important thing to consider. Try to work on medium or high priority issues
	  as much as possible. However, if you know you're going to have a busy week ahead, maybe go for one of the lower
	  priority issues so you don't hold up development in the rest of the game.
	- It's generally recommended to only fix 1 issue per PR. If you feel like you can split up an issue into multiple
	  PRs then do that. Avoid fixing more than one issue unless you really know what you're doing and they're all
	  smaller changes.
2. Pull and checkout master.
3. Create a new branch.
	- Branch names should be in lowercase with `-` instead of spaces. Try to name it after the issue(s) you're fixing.
4. Code and fix the issue!
	- Have a read of the [Developing](Developing.md) guide for best practices when developing a PR.
	- Commit and push your commits to the remote early and often.
	- Try to write meaningful commit messages.
	- Rebase onto master as soon as anyone else merges a PR. Please see the section on [rebasing](#How to rebase)
5. Once you've finished fixing the issue, test it yourself. Make sure all temporary components are removed and the
   correct things are enabled.
6. Rebase onto master again for good measure.
7. Make a PR on Github
	- Include a description that details what you did and how you did it.
	- Add appropriate labels to the PR.
	- If the PR resolves an issue, add `Fixes #X` somewhere in the description, where `X` is the number of the issue you
	  fixed.
8. Go to the `Files changed` tab and read over **ALL** of your changes, making sure there are no mistakes.
9. Ask someone to review your PR. You can request reviewers by adding them to the reviewers.

## How to rebase

### DO NOT MERGE. I REPEAT, DO NOT MERGE. YOU SHOULD BE SCARED OF THE WORD "MERGE"

When rebasing a PR, please follow these instructions very, very carefully. Ideally keep this document open while you are
doing so.

1. Ensure all important gameobjects are saved as prefabs somewhere.
2. Update master.
3. Press `rebase current onto selected`. **DO NOT PRESS MERGE**
4. Wait for the rebase to happen.
	- There will probably be some rebase conflicts, these are normal and a dialog will appear if they exist where you
	  can resolve them.
		- For all rebase conflicts in C# files, double click and look at the local and master changes. Figure out what
		  you need to keep and how to combine both changes.
		- For all other rebase conflicts (Scenes, Prefabs), you can try to double click and combine them, but it is
		  often hard to do so. In this case, the best practice is to `accept theirs` and then redo your changes later.
5. Immediately force-push your branch. **DO NOT PRESS UPDATE OR PULL REMOTE CHANGES**

## When "merging" is ok

By now, you should be scared of the word "merge". If you are not, please read the previous section again and again until
you have a inner fear of that word.

In certain scenarios though, you will need to "merge":

- Combining changes when rebasing is usually called "merging" the file.
- "Merging" a PR into master is acceptable.

If you are in doubt whether you should merge or not, stop and ask someone on the Discord.

## Reviewing a PR

Don't be afraid to review a PR, because not only do you help someone else get their changes merged, but you also learn
about how other parts of the game work and techniques that you could apply when coding yourself. Try and review as many
PRs as possible.

Steps to review a PR:

1. Pull the branch locally.
2. Test out the game. Do the actual changes line up with what you expected from the PR? Is anything off? Are there any
   bugs?
	- Try and test edge cases: what if the player has no ammo left? Does it sync on multiplayer?
3. Read every line of C# code. Comment on anything that differs from how you would implement the feature.
	- If you don't understand something, leave a comment asking them to explain it.
	- Comment on any repetition.
	- Would this code be easy to build on in the future? How easy would it be to make a change?
4. Add any comments about the game or non-script stuff in the review comment.
5. If something is implemented well, say so! Don't just be critical!
6. If everything looks good, approve it. Otherwise, request changes.

If everything seems to break, don't immediately blame it on the PR. Instead, do these things first:

- Check you've fetched **and** pulled the branch. On rider you need to press the blue arrow to fetch the repository
  before pulling.
- Restart Rider and Unity
	- Important if you're getting `Missing Reference` errors in Rider or any spam errors in Unity.