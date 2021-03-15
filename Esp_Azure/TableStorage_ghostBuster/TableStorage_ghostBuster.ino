#include <Esp32MQTTClient.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <ArduinoJson.h>
#include <WiFi.h>
#include "time.h"

char msg[512];
int reelSwitch = 4;
int switchState;

const char* ntpServer = "europe.pool.ntp.org";
const long  gmtOffset_sec = 3600;
const int   daylightOffset_sec = 3600;
static char* connectionString = "HostName=iot-robban.azure-devices.net;DeviceId=GhostBuster;SharedAccessKey=Cs7YlKHHp+QRXQchjPGmpMLqFCreLbGdyTMeLTVh74s=";
static bool _connected = false;


void initIotHub() {
  if (!Esp32MQTTClient_Init((const uint8_t * ) connectionString)) {
    _connected = false;
    
    return;
  }
  _connected = true;
}

char* _id = "AC:67:B2:3F:68:C0";
char* ssid = "Robin's Wi-Fi Network";
char* pass = "Batman123";



void SendConfirmationCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result) {
  if (result == IOTHUB_CLIENT_CONFIRMATION_OK) {
    Serial.println("Confirmed");
  }
  else{
    Serial.println("Not confirmed");
  }
}

void sendIotMessage(char* msg) {
  EVENT_INSTANCE* message = Esp32MQTTClient_Event_Generate(msg,MESSAGE);
  Esp32MQTTClient_Event_AddProp(message,"Name","Robin Ellingsen");
  Esp32MQTTClient_Event_AddProp(message,"School","Nackademin");
  Esp32MQTTClient_SendEventInstance(message);
}

unsigned long printLocalTime()
{
  time_t now;
  
  struct tm timeinfo;
  if(!getLocalTime(&timeinfo)){
    Serial.println("Failed to obtain time");
    return 0;
  }
  Serial.println(&timeinfo, "%A, %B %d %Y %H:%M:%S");
  time(&now);
  return now;
}



void setup() {
  Serial.begin(115200); 
  pinMode(5, OUTPUT);
  pinMode (reelSwitch, INPUT);
   WiFi.begin(ssid, pass);
  Serial.println("Connecting");
  while (WiFi.status() != WL_CONNECTED) 
  {
    Serial.print(".");
    delay(1000);
  }
  Serial.println("Connected to WiFi");
  initIotHub();

  
  
  if(Esp32MQTTClient_Init((const uint8_t *) connectionString))
  {
    Serial.println("Connected to MQTT Client");
  }
  Esp32MQTTClient_SetSendConfirmationCallback(SendConfirmationCallback);
  configTime(0, 0, ntpServer);
}


void loop() {
    DynamicJsonDocument doc(512);
    JsonObject root = doc.to<JsonObject>();
    switchState = digitalRead(reelSwitch);
  
  {
      root["type"] = "GhostBuster";
      root["Activity"] = true;
      root["TimeStamp"] = printLocalTime();
      root["deviceId"] = _id;
      digitalWrite(5, HIGH); 
      delay(1000);
      digitalWrite(5, LOW);
      serializeJson(root, msg);
      Serial.print(msg);
      sendIotMessage(msg);
      delay(10000);
  }                
}
