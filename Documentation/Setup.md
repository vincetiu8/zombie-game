# Setting up the project

Before coding anything, please follow this guide to get the project set up on your computer. Don't download this
repository yet, follow the instructions below in that order. If anything in this document doesn't line up with what you
experience, please ask for help in the discord server.

### Installing and configuring software

Please don't start downloading anything until you've finished reading the instructions. Afterwards, proceed with the
downloads and refer back to these steps as necessary. Downloading multiple things at a time speeds up the process, so
please follow the sections below in parallel.

##### Pre-requisites

It's assumed that you have a Github account that has access to this repository, otherwise you wouldn't be seeing this
file! You should also have sufficient storage space (~8 GB) to install everything.

##### Jetbrains Rider

Rider is the preferred IDE to use with the project. The school has a free educational license linked to your account.
Feel free to use your own IDE for editing the code, but please use Rider to perform the final checks, code formatting
and all commits.

1. Register for a [JetBrains educational license](https://www.jetbrains.com/community/education/#students).
    1. If you're not from BSM, you may need to add your school to
       the [JetBrains SWOT repository](https://github.com/JetBrains/swot). Get someone to help if you're not confident
       with git.
2. Download [Rider](https://www.jetbrains.com/rider/download/).
3. Run installer, use default options.
4. Open Rider
5. When the splash screen displays, select `Get From VCS`.   
   ![](Images/SetupImage1.png)
6. Select an appropriate directory. No need to create a new one, Rider will do that.
7. Copy this [url](https://github.com/vincetiu8/zombie-game.git) into the URL field.  
   ![](Images/SetupImage2.png)
8. Rider will prompt you to create a Github token. Press `Generate`, and it will automatically create a Github token for
   you. You need to confirm the creation of the token. It will then bring you into a menu with the exposed token. Copy
   the token into the token field and press `Add Account`.  
   ![](Images/SetupImage3.png)
9. Wait for the project to clone.
10. Go to `File > Settings` on the top left, then select `Editor > Code Style`. Change the scheme to `Project`.  
    ![](Images/SetupImage4.png)
11. Still in settings, go to `Version Control > Diff & Merge > External Diff Tools`. `Enable external merge tool` and
    set the executable path to `{ADD YOU UNITY INSTALL PATH HERE}/Editor/Data/Tools/UnityYAMLMerge`. If on windows,
    add `.exe` to the end.  
    ![](Images/SetupImage5.png)
12. Close Rider and open the project from Unity Hub, selecting the installed version of Unity. Wait for it to load.
13. Go to `Edit > Preferences` and then select `External Tools`. Set the external script editor to Rider.
    ![](Images/SetupImage6.png)

##### Unity and Unity Hub

Unity is the game engine we're using to code our zombie game. Unity Hub is used to manage Unity versions. We change
versions regularly so please download Unity Hub and then download Unity from there.

1. Download [Unity Hub](https://store.unity.com/download-nuo).
    1. While waiting create a unity account with your school email.
2. Run installer, use default options.
3. Install Unity 2020.3.17f1.
    1. When prompted to activate or specify a license, activate a new personal one.
    2. When selecting add-ons, select the platform you're running on (Windows, Mac, Linux). Documentation is optional.

##### Git LFS

Git LFS makes using git with large files easier.

1. Download the [git LFS](https://git-lfs.github.com/) installer.
2. Run installer, use default options.
