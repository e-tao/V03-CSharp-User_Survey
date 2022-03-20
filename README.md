# V03 C# Project User Survey

## Introduction
A database-drive GUI application written in C#.

The application requirements:
- the application needs to have 2 modes: survey mode and results mode;
- survey mode:
    - ask user name and at least 3 other information;
    - survy questions are loaded from database;    
    - question use same nemerical scale;
    - store the results in database;
- results mode:
    - Show a summative view of the response data;
    - Show valid results only (completed survey);
    - Show statistics by user type (gender/age group);

## Features 
The application has the following features:
- takes user name (first, last), gender, email address, and age group information;
- imcompleted survey will be deleted;
- user can only see the results after complete the survey;
- add (20, 50, or 100) random user from random user api and their survey results to database for better data visulization;


## Tech
The project uses the following language, toolkit, IDE, database etc...

- [C#] - The sole language for the application
- [WPF] -  Graphical class library 
- [MariaDB] - RDBMS
- [Visual Studio] - IDE for C# development
- [Material Design XAML Toolkit] - GUI libraries for WPF
- [git] - version control

## Screenshot
![Application Screenshot](https://github.com/ethantao-repo/V03-PRJ-User_Survey/blob/master/screenshot/V03_Project_ScreenShoot.png?raw=true)
