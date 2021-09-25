# Developing Guide

Here are some useful tips when developing features.

## Code (C#) related

### Concepts

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

### Styling

- Try to expose the least amount of variables and methods.
	- Use `[SerializeField]` if a value needs to be edited in the editor, but not accessed during runtime.
- Return early.
	- Avoid `else` statements if possible.
- Follow all rider suggestions.

### Formatting

- Add comments on anything that isn't obvious.
	- Use comments to explain complex methods.
	- Comments should have a space and a capital, with no full stop: `// This is an example comment`
	- Weird variables, methods and classes should have the `<summary>` comment to explain them.
		- Rider automatically adds this if you insert `///`.
- Add `#regions` to break up a file into (collapsible) areas.
- Add Inspector attributes to everything.
	- Add `[Header]` to separate sections, even if there is only one.
	- Every variable that isn't an objects reference should have a `[Description]`.
	- All numerical variables should have a `[Range]` and default value.

## Unity related

- Make things prefabs.
- Test everything.
	- Make sure to test network syncing too.

## Git related

- Commit and push early and often.
- Don't touch things you don't need to touch!
	- Try to implement everything while touching the least files possible. Makes it easier to review and diff.
- Rebase every time someone merges a new commit into master.

### Formatting

- Make good commit, issue and PR messages. Describe what you did in each commit.
    - Ideally, no `asdfghjk`, it confuses people reading your code.
- Always write in present tense.
  - `Add` not `Added` etc.
- Issue and PR titles should be in normal english.
  - First letter capital and the rest lowercase (if it's not a name/acronym).

## Useful links

### Unity

- [Scripting API](https://docs.unity3d.com/ScriptReference/)

### Photon

- [PUN API Reference](https://doc-api.photonengine.com/en/pun/v2/index.html)
- [Synchronization and state](https://doc.photonengine.com/en-us/pun/current/gameplay/synchronization-and-state)
- [PUN Cheatsheet](https://gist.github.com/ssshake/86b4da6c31258a7188f7fef3dbaf1d26)

### Git and Github

- [Vince's Tutorials](https://drive.google.com/drive/folders/1V6t2gXSj55A1u6bt9WzTS97c2CJlKpg7)