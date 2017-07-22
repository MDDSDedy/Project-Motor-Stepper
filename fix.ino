#define ENPOLA  12 //PIN ENABLE A
#define ENPOLB  9 //PIN ENABLE B
#define POLAP   13 //PIN A
#define POLAM   11 //PIN A_BAR
#define POLBP   10 //PIN B
#define POLBM   8 //PIN B_BAR

String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete
int  pil = 0, a = 0;
int pA = 0, pB = 0;
String kontrol = "";
float sud = 0;
char buff[6];
void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  inputString.reserve(20);
  kontrol.reserve(20);
  pinMode(ENPOLA, OUTPUT);
  //pinMode(13, OUTPUT);
  pinMode(ENPOLB, OUTPUT);
  pinMode(POLAP, OUTPUT);
  pinMode(POLAM, OUTPUT);
  pinMode(POLBP, OUTPUT);
  pinMode(POLBM, OUTPUT);
  digitalWrite(ENPOLA, LOW);
  digitalWrite(ENPOLB, LOW);
  digitalWrite(POLAP, LOW);
  digitalWrite(POLAM, LOW);
  digitalWrite(POLBP, LOW);
  digitalWrite(POLBM, LOW);
}

void loop() {
  // put your main code here, to run repeatedly:
  if (stringComplete) {
    //#-100*-100
    pA = inputString.substring(1, 5).toInt();
    pB = inputString.substring(6, 10).toInt();
    // clear the string:
    inputString = "";
    stringComplete = false;
    digitalWrite(ENPOLA, HIGH);
    digitalWrite(ENPOLB, HIGH);
    if (pA >= 0)  {
      //Serial.print("POLAP  ");
      analogWrite(POLAP, pA);
      analogWrite(POLAM, 0);
    } else {
      //Serial.print("POLAM  ");
      analogWrite(POLAP, 0);
      analogWrite(POLAM, abs(pA));
    }

    if (pB >= 0)  {
      //Serial.println("POLBP");
      analogWrite(POLBP, pB);
      analogWrite(POLBM, 0);
    } else {
      //Serial.println("POLBM");
      analogWrite(POLBP, 0);
      analogWrite(POLBM, abs(pB));
    }
  }
}

void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
    // add it to the inputString:
    else
    {
      inputString += inChar;
    }
  }
}
