#!/bin/bash

# Example curl command
curl --request POST \
     --url https://api.gameshift.dev/asset-templates \
     --header 'accept: application/json' \
     --header 'content-type: application/json' \
     --header 'x-api-key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJrZXkiOiJjZDQ3MDg3YS05NWE0LTQwODAtOTI3Ni05MGViMzRhNTJkMWUiLCJzdWIiOiJhNTVmYWY2YS0yNWMyLTQ4ZTgtODgzNi1jYjEyZGU0MTUyNTQiLCJpYXQiOjE3MDg2MTQ5NjR9.G7obkhWbK5N1s-vhlh3gdy-19vTuowyGpvROcNkC0h0' \
     --data '
{
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

curl --request POST \
     --url https://api.gameshift.dev/asset-templates \
     --header 'accept: application/json' \
     --header 'content-type: application/json' \
     --header 'x-api-key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJrZXkiOiJjZDQ3MDg3YS05NWE0LTQwODAtOTI3Ni05MGViMzRhNTJkMWUiLCJzdWIiOiJhNTVmYWY2YS0yNWMyLTQ4ZTgtODgzNi1jYjEyZGU0MTUyNTQiLCJpYXQiOjE3MDg2MTQ5NjR9.G7obkhWbK5N1s-vhlh3gdy-19vTuowyGpvROcNkC0h0' \
     --data '
{
  "sourceImage": "https://drive.google.com/file/d/1Swzb7B0T4_l4WnmFeDo6wBk0sjtgX9rM/view?usp=sharing",
  "name": "red character",
  "description": "red"
}
'

curl --request POST \
     --url https://api.gameshift.dev/asset-templates \
     --header 'accept: application/json' \
     --header 'content-type: application/json' \
     --header 'x-api-key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJrZXkiOiJjZDQ3MDg3YS05NWE0LTQwODAtOTI3Ni05MGViMzRhNTJkMWUiLCJzdWIiOiJhNTVmYWY2YS0yNWMyLTQ4ZTgtODgzNi1jYjEyZGU0MTUyNTQiLCJpYXQiOjE3MDg2MTQ5NjR9.G7obkhWbK5N1s-vhlh3gdy-19vTuowyGpvROcNkC0h0' \
     --data '
{
  "sourceImage": "https://drive.google.com/file/d/1LHD6jxhQug-Hl8wQt0oMYHS09kQN7mqy/view?usp=sharing",
  "name": "blue character",
  "description": "blue"
}
'

curl --request POST \
     --url https://api.gameshift.dev/asset-templates \
     --header 'accept: application/json' \
     --header 'content-type: application/json' \
     --header 'x-api-key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJrZXkiOiJjZDQ3MDg3YS05NWE0LTQwODAtOTI3Ni05MGViMzRhNTJkMWUiLCJzdWIiOiJhNTVmYWY2YS0yNWMyLTQ4ZTgtODgzNi1jYjEyZGU0MTUyNTQiLCJpYXQiOjE3MDg2MTQ5NjR9.G7obkhWbK5N1s-vhlh3gdy-19vTuowyGpvROcNkC0h0' \
     --data '
{
  "sourceImage": "https://drive.google.com/file/d/1u0kSo4g8t2b31qHC7ibNJLwz-fRJl2Un/view?usp=sharing",
  "name": "white character",
  "description": "white"
}
'
