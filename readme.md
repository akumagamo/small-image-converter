# Mini Image Converter
## Versionnumber 0.1.0 (2024-04-26)

Small tool to create png's and gif's from an SVG.  
All the configuration is done in the `appsettings.js`. You only can specify an input and output-folder and the desired sizes for the convered files.  
And The application assumes the input svg ist atleat 256x256 and is a square image _(the output are also square images)_.

![Screenshot from a config](https://raw.githubusercontent.com/akumagamo/small-image-converter/master/readme/screenshot_0001.png "Screenshot from a config")  

## Features
* Creates PNG and GIF version of an SVG in the output folder
* Moves SVG after successful run into the outputfolder
* When started will execute tasks for each SVG in the input Folder

## Roadmap / Future Features
* None

## Known Bugs
* None

## SourceControl Link & Information
https://github.com/akumagamo/small-image-converter.git

## NuGet Packages used
_for details check the Project file_

    * Microsoft.Extensions.Configuration
    * Microsoft.Extensions.Configuration.Json
    * Svg

## Config Overview   
    {
      "AppSettings": {
        "InputFolder": "input",   // Inputfolder for the SVG Files
        "OutputFolder": "output", // Outputfolder for all Images
        "PngFormats": [ 256, 48, 32 ], // different OutputSizes for PNG's (number = width and height)
        "GifFormats": [ 32 ] // different OutputSizes for Gif's (number = width and height)
      }
    }