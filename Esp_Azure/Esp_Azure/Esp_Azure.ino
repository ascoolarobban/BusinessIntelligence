#include <Esp32MQTTClient.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <ArduinoJson.h>
#include <WiFi.h>
#include "time.h"

#define DHT_PIN 4
#define DHT_TYPE DHT11
#define Photoresistor 35

int interval = 6 * 1000;
const char* ntpServer = "europe.pool.ntp.org";
const long  gmtOffset_sec = 3600;
const int   daylightOffset_sec = 3600;
int ledPin = 5;
static char* connectionString = "HostName=iot-robban.azure-devices.net;DeviceId=esp32;SharedAccessKey=GiSqmjq/DLaGYKuhP/+nogcpoF+JZq7h6REFLs3pGrM=";
static bool _connected = false;

float temp = 0;
float prev = 0;
float diff = 1;



void initIotHub() {
  if (!Esp32MQTTClient_Init((const uint8_t * ) connectionString)) {
    _connected = false;

    return;
  }
  _connected = true;
}
char* _id = "AC:67:B2:26:67:7C ";
char* ssid = "Robin's Wi-Fi Network";
char* pass = "Batman123";

bool messagePending = false;

char msg[512];
DHT dht(DHT_PIN, DHT_TYPE);

void SendConfirmationCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result) {
  if (result == IOTHUB_CLIENT_CONFIRMATION_OK) {
    Serial.println("Confirmed");
    messagePending = false;
  }
  else {
    Serial.println("Not confirmed");
  }
}


void sendIotMessage(char* msg) {
  EVENT_INSTANCE* message = Esp32MQTTClient_Event_Generate(msg, MESSAGE);
  Esp32MQTTClient_Event_AddProp(message, "Name", "Robin Ellingsen");
  Esp32MQTTClient_Event_AddProp(message, "School", "Nackademin");
  Esp32MQTTClient_SendEventInstance(message);
}
unsigned long printLocalTime()
{
  time_t now;

  struct tm timeinfo;
  if (!getLocalTime(&timeinfo)) {
    Serial.println("Failed to obtain time");
    return 0;
  }
  Serial.println(&timeinfo, "%A, %B %d %Y %H:%M:%S");
  time(&now);
  return now;
}


void setup()
{

  Serial.begin(115200);
  WiFi.begin(ssid, pass);
  Serial.println("Connecting");
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(1000);
  }
  Serial.println("Connected to WiFi");
  initIotHub();
  dht.begin();



  if (Esp32MQTTClient_Init((const uint8_t *) connectionString))
  {
    Serial.println("Connected to MQTT Client");
  }
  Esp32MQTTClient_SetSendConfirmationCallback(SendConfirmationCallback);
  configTime(0, 0, ntpServer);

  pinMode (ledPin, OUTPUT);


}
bool difference() {
  if (temp > (prev + diff) || temp < (prev - diff)) {
    prev = temp;
    return true;
  }
  return false;
}

void loop() {
  digitalWrite(ledPin, LOW);
  temp = dht.readTemperature();
  float hum = dht.readHumidity();
  int analog_value = analogRead(Photoresistor);
  int brightness = map(analog_value, 0, 4096, 0, 100);
  delay(10);

  DynamicJsonDocument doc(512);
  JsonObject root = doc.to<JsonObject>();
  if (difference()) {
    root["DeviceName"] = "Esp32";
    root["ID"] = _id;
    root["Temperature"] = temp;
    root["Humidity"] = hum;
    root["Brightness"] = brightness;
    root["Longitude"] = "59.31462544258207";
    root["Latitude"] = "18.086804195131226";
    root["TimeSent"] = printLocalTime();


    serializeJson(root, msg);




  //  if (!messagePending)
  //  {
  //    messagePending = true;
      sendIotMessage(msg);
      Serial.print(msg);
      digitalWrite(ledPin, HIGH);
      delay(1000);
      digitalWrite(ledPin, LOW);
    //  delay(6000);

 //   }

  }
}
