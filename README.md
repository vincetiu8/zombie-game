# The BSM Byte Surfers Zombie Game

Welcome to the BSM Byte Surfers Zombie Game github repository! We're looking forward to working together on this project
and creating something amazing!

### Where to start

1. Follow the instructions in [Setup](Documentation/Setup.md) to get everything configured on your computer.
2. Read [Workflow](Documentation/Workflow.md) and [Developing](Documentation/Developing.md)
3. **Make sure you've pulled the latest master**.
4. Find a small issue (xs/s) and assign yourself to it.
5. Fix the issue.
6. Make a PR and request for review!

### Issues

Create issues early and often. Issues in Github (and software development) aren't always problems: most issues will be
adding or upgrading new features. More or less anything you'd like to change is an issue, and you should create them as
often as possible.

When making an issue, also think about how you would go about implementing it, what changes you would make and what
structures or models any new features would have. Be as descriptive as possible.

It is also preferred to break up an issue into lots of smaller issues. For example, instead of creating a larger issue
on making a shop, create some smaller issues on:

- Adding a currency to the game
- Having enemies drop said currency on death
- Adding a vending machine to the game
- Allowing vending machines to sell different items

After writing the issue, you should give it relevant labels. All issues should have an `area`, `size` and `type`.

Priority is how important the issue is in relation to the game and other issue. Try to assign yourself to medium or high
priority issues.  
| Priority | Description | Example |
|---|---|---|
| High | Work on this if possible. Relates to several other issues. | Adding a weapon system |
| Medium | Work on this if interesting. Relevant and useful. | Adding a new enemy |
| Low | Work on this if nothing else is available. Not strictly needed. | Adding armor |
| None | Don't work on this. Often depends on a pending issue. | Adding a perk (without perk system yet) |

Areas are what aspect of the game the issue relates to. These include:  
| Area | Description | Example |  
|---|---|---|  
| Collectibles | Items players can pick up | Adding a power-up |  
| Enemies | Enemies and attacks | Adding a new enemy |  
| Map | Map and objects | Adding a new map area |  
| Meta | Not relating directly to the game | Updating documentation |  
| Networking | Multiplayer and connectivity | Creating a lobby system |  
| Player | Movement and input | Adding a new movement ability |  
| Shop | Shop and currency | Adding a new vending machine |  
| UI | User interface | Adding a health bar |  
| Weapons | Weapons players can use | Adding a shotgun |

Sizes are used to estimate the time needed to fix an issue. This is measured in hours actually spent coding: an extra
large issue should take 24 hours or more of straight coding to complete. When sizing an issue, think of how many hours
it would take and experienced coder to complete. These include:  
| Size | Symbol | Estimated Time Needed |  
|---|---|---|  
| Extra small | xs | < 1 hour |  
| Small | s | 1 - 2 hours |  
| Medium | m | 3 - 4 hours |  
| Large | l | Half a day |  
| Unsure | ? | Unsure of length |

Types are used to identify whether the issue is a bug, enhancement or feature. These include:
| Type | Description | Example |  
|---|---|---|  
| Bug | Something isn't working | Player can't shoot |  
| Refactor | Cleaning up implementation | Changing objects to use same parent class |  
| Enhancement | Updating an existing feature | Adding another weapon |  
| Feature | Adding a new feature | Adding a perk system |

### Pull Requests

The two rules:

1. **NEVER MERGE**
2. **KEEP PRs and COMMITS SMALL**

If you follow theses rules, everything will be ok.

As soon as you start work on a new feature, even before you make a commit, push your branch.

Once you're done making all the changes, immediately make a PR and label it accordingly. You should review your code
changes yourself (you can ignore the `.meta` files and any scene/prefab/other asset changes), and if you've done
anything confusing leave a review comment. Only after going through all your files yourself should you request for a
review.
