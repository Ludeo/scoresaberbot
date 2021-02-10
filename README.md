# Score Saber Discord Bot
This is a private discord bot that me and some friends are using. If you want to host the bot for yourself, jump to [here](#selfhosting-instructions) and follow the steps.


# Score Saber API
All the information that I got about the API is from [ppotatoo's](https://github.com/ppotatoo) wiki that you can find [here](https://github.com/ppotatoo/ssapi/wiki). Thanks for figuring that stuff out ppotatoo ^^


# Selfhosting Instructions
Download the newest release and run the bot. The bot should have instantly closed and created a config.xml file now. Fill out all of the parameters and run the bot again (If you don't fill out everything, the bot might not work correctly). Now everything should work and the bot will create missing xml files, when he needs them. For the possible config parameters, go to [here](#config-parameters).


# Config parameters
    "token": Discord bot token, should be self explanitory
    "prefix": Prefix for commands
    "adminid": Your discord id, so you can run admin only commands
    "trackserver": The id of the server where you want to track recent scores
    "trackchannel": The id of the channel from the trackserver, where the bot should send in recent scores


# Commands
## Admin only
### addtrack [scoresaberid]
Adds the score saber account to the tracking system
### deletetrack [scoresaberid]
Deletes the score saber account from the tracking system
### logout
Logs out the bot so he is instantly offline before closing the program
### starttrack
Starts the tracking system
### stoptrack
Stops the tracking system
## Everyone
### credits
![](https://i.imgur.com/geb3pel.png)
![](https://i.imgur.com/tn2iscN.png)
### help
![](https://i.imgur.com/fNV4T1n.png)
### leaderboard
![](https://i.imgur.com/9uM3AjZ.png)
![](https://i.imgur.com/b8QkEEA.png)
### link
![](https://i.imgur.com/9upt2qx.png)
### profile
![](https://i.imgur.com/C8CdGyj.png)
![](https://i.imgur.com/lzGMLNg.png)
### recent
![](https://i.imgur.com/L980TyH.png)
![](https://i.imgur.com/hUwpMvw.png)
### top5
![](https://i.imgur.com/9kl6iJ6.png)
![](https://i.imgur.com/L7EQU7V.png)
### top
![](https://i.imgur.com/IKPnf0e.png)
![](https://i.imgur.com/jgWMA64.png)