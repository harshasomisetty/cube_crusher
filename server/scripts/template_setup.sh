#!/bin/bash

# Define API key variable
API_KEY="YOUR_KEY"

# Function to perform POST request
post_asset_template() {
    curl --request POST \
         --url https://api.gameshift.dev/asset-templates \
         --header "accept: application/json" \
         --header "content-type: application/json" \
         --header "x-api-key: $API_KEY" \
         --data "$1"
}

# JSON data for each asset
DATA1='{
  "sourceImage": "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSa2Dlm0rQfd_5Bm6HB3Ux3F3jmpqYy7x5nkQ&usqp=CAU",
  "name": "Profile",
  "description": "Stores Profile Data",
  "attributes": [
    {
      "traitType": "Games Played",
      "value": "0"
    }
  ]
}'

DATA2='{
  "sourceImage": "https://drive.google.com/file/d/1Swzb7B0T4_l4WnmFeDo6wBk0sjtgX9rM/view?usp=sharing",
  "name": "red character",
  "description": "red"
}'

DATA3='{
  "sourceImage": "https://drive.google.com/file/d/1LHD6jxhQug-Hl8wQt0oMYHS09kQN7mqy/view?usp=sharing",
  "name": "blue character",
  "description": "blue"
}'

DATA4='{
  "sourceImage": "https://drive.google.com/file/d/1u0kSo4g8t2b31qHC7ibNJLwz-fRJl2Un/view?usp=sharing",
  "name": "white character",
  "description": "white"
}'

# Post each asset
post_asset_template "$DATA1"
post_asset_template "$DATA2"
post_asset_template "$DATA3"
post_asset_template "$DATA4"
