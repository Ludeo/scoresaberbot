# Score Saber Discord Bot
This is a private discord bot that me and some friends are using. If you want to host the bot for yourself, jump to [here](#selfhosting-instructions) and follow the steps.


# Selfhosting Instructions
Download the repository and run the code. The bot should have created a config.xml file now. Fill out all of the parameters and run the bot again (If you don't fill out everything, the bot might not work correctly). Now everything should work and the bot will create missing xml files, when he needs them. For the possible config parameters, go to [here](#config-parameters).


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
### starttrack
Starts the tracking system
### stoptrack
Stops the tracking system
## Everyone
### credits
![](https://i.imgur.com/OtX8X47.png)
### link
![](https://i.imgur.com/pqpu2v9.png)
### profile
![](https://i.imgur.com/Yny5z8g.png)
### recent
![](https://i.imgur.com/Ajh7A3E.png)
### top5
![](https://i.imgur.com/izgK2qQ.png)
### top
![](https://i.imgur.com/fPhjn8X.png)
### updatename
![](https://i.imgur.com/kBuH3wR.png)
