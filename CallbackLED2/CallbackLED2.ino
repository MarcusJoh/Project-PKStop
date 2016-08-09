/*
  Copyright (c) 2015 Intel Corporation. All rights reserved.

  This library is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public
  License as published by the Free Software Foundation; either
  version 2.1 of the License, or (at your option) any later version.

  This library is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
  Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public
  License along with this library; if not, write to the Free Software
  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-
  1301 USA
*/
byte val = 6;
byte oldval = 6;

#include <CurieBLE.h>

#include <EducationShield.h>

IMU imu; // the IMU
long lastTimer; //used for saveing millis()
bool up, down, left, right;
byte dir;

const int ledPin = 13; // set ledPin to use on-board LED
BLEPeripheral blePeripheral; // create peripheral instance

BLEService ledService("19B10000-E8F2-537E-4F6C-D104768A1214"); // create service

// create switch characteristic and allow remote device to read and write
BLECharCharacteristic switchChar("19B10001-E8F2-537E-4F6C-D104768A1214", BLERead | BLEWrite);

void setup() {
  Serial.begin(9600);
  /* Initialise the IMU */

  imu.begin();
  imu.detectShock();
  imu.attachCallback(shockCallback);

  pinMode(ledPin, OUTPUT); // use the LED on pin 13 as an output

  // set the local name peripheral advertises
  blePeripheral.setLocalName("LEDCB");
  // set the UUID for the service this peripheral advertises
  blePeripheral.setAdvertisedServiceUuid(ledService.uuid());

  // add service and characteristic
  blePeripheral.addAttribute(ledService);
  blePeripheral.addAttribute(switchChar);

  // assign event handlers for connected, disconnected to peripheral
  blePeripheral.setEventHandler(BLEConnected, blePeripheralConnectHandler);
  blePeripheral.setEventHandler(BLEDisconnected, blePeripheralDisconnectHandler);

  // assign event handlers for characteristic
  // switchChar.setEventHandler(BLEWritten, switchCharacteristicWritten);
  // set an initial value for the characteristic
  switchChar.setValue(0);

  // advertise the service
  blePeripheral.begin();
  Serial.println(("Bluetooth device active, waiting for connections..."));
}

void loop() {
  // poll peripheral
  blePeripheral.poll();

  // read raw rollro measurements from device
  imu.run();
  // get the pitch and roll result from the filter
  int pitch = imu.getPitch();
  int roll = imu.getRoll();



  // check if the board has been tilted in any direction and set the direction bools accordingly
  if (pitch < -45) {
    up = true;
    dir = 2;
  } else if (pitch > 45) {

    down = true;
    dir = 1;

  } else if (roll  < -45) {

    left = true;
    dir = 3;
  } else if (roll > 45) {

    right = true;
    dir = 4;
  } else {

    dir = 5;
  }


  
 


  Serial.print(dir);
 
   
    if(oldval != val) {
      dir = val; Serial.println(dir);

  }
 
switchChar.setValue(dir);
  oldval = val;
delay(350);
}

void blePeripheralConnectHandler(BLECentral& central) {
  // central connected event handler
  Serial.print("Connected event, central: ");
  Serial.println(central.address());
}

void blePeripheralDisconnectHandler(BLECentral& central) {
  // central disconnected event handler
  Serial.print("Disconnected event, central: ");
}
static void shockCallback(void)
{

  val++;

}

