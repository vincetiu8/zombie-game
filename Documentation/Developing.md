# Developing Guide

Here are some useful tips when developing features.

## Code (C#) related
- Don't Repeat Yourself.
  - You should never have to write anything twice.
  - Think about combining common code into methods.
    - If the code spans across classes, could it be put in `Utils`?
- Try to make things easy for others to work with.
  - Make things extensible. Even if it's a little more work now, it will save a lot of time and stress in the future.
  - Always think about future features that might be added.
  - Make use of inheritance and overloading.
- Avoid spaghetti code.
  - Ideally, object references should only go one-way. You shouldn't have any objects that both reference each other.
- Return early.
  - Avoid `else` statements if possible.

## Unity related
- Make things prefabs.
- Test everything.
  - Make sure to test network syncing too.

## Git related
- Commit and push early and often.
- Make good commit messages. Describe what you did in each commit.
  - Ideally, no `asdfghjk`, it confuses people reading your code.
- Don't touch things you don't need to touch!
  - Try to implement everything while touching the least files possible. Makes it easier to review and diff.
- Rebase every time someone merges a new commit into master.