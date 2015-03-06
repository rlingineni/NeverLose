// demo of Grove - Starter V2.0
// Loovee  2013-3-10
// this demo will show you how to use Grove - Button to control a LED
// when the button was pressed, the led will on 
// otherwise led off
// Grove - Button connect to D3
// Grove - LED connect to D7

const int pinButton = 2;                        // pin of button define here
const int pinLed    = 7;                        // pin of led define here

void setup()
{
    pinMode(pinButton, INPUT);                  // set button INPUT
    pinMode(pinLed, OUTPUT);   
Serial.begin(9600);    // set led OUTPUT
}

void loop()
{
    if(digitalRead(pinButton))                     // when button is pressed
    {
        digitalWrite(pinLed, HIGH);   
        Serial.write("1");
       
        // led on
    }
    else
    {
        digitalWrite(pinLed, LOW);
        Serial.write("0");
       
    }
    
    delay(1000);
}
