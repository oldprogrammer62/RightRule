### The Equals Operator

## Recognized keywords
- equals
- eq
- =
- ==

## Usage
The Equals Operator performs equality between 2 values.  

It can handle the following data types:  
- ## Number
When both variable and value are numbers  
Example: 10 = 10
- ## String
When both variable and value are strings  
String equality is case insenitive.
"abc" = "abc"
- ## Char
When both variable and value are char  
'a' = 'a'  
- ## Boolean
When both variable and value are boolean true or false  
true = true  
When both variable and value are strings "false" or "true"  
"true" = "true"  
When one of variable or value is a boolean and the other is a string "false" or "true"  
true = "true"  
- ## Datetime
When both variable and value are Datetime  
"dd.MM.yyyy HH:mm:ss" = "dd.MM.yyyy HH:mm:ss"  
- ## Dateonly
When both variable and value are Dateonly  
"dd.MM.yyyy" = "dd.MM.yyyy"  
- ## Timeonly
When both variable and value are Timeonly  
"HH:mm:ss" = "HH:mm:ss"  

Any other types or combinations will be handled as equality between dynamic variables  

