# IotPinBall
Win 10 IoT Core App for MMS Session IoT Pinball



######TODO
* Hook up to Acutal PinBall Machine - read score values
* Send to IoT Hub / PowerBI
* LOTS of other work - Visual to make it look like a pinbal display

This is a Windows 10 Universal Application designed to read score data from a pinball machine. A user will enter their name before they start and then click start game. The Score will actively be read from the i2c port, and when the game is complete the user will click "game over" The score, name, timing, etc. will then be stored locally and pushed to the Iot hub.

####Main Menu / Waiting for Player
![alt tag](https://raw.githubusercontent.com/bergotronic/IotPinBall/master/Screenshots/1.png)

####Game in Progress - Current Score - Game Over Button
![alt tag](https://raw.githubusercontent.com/bergotronic/IotPinBall/master/Screenshots/2.png)

####Game Over Screen and reset
![alt tag](https://raw.githubusercontent.com/bergotronic/IotPinBall/master/Screenshots/3.png)

###Local Data Logging
![alt tag](https://raw.githubusercontent.com/bergotronic/IotPinBall/master/Screenshots/4.png)


