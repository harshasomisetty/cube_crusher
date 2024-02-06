const express = require('express');
const axios = require('axios');
const app = express();

const port = process.env.PORT || 3001;

require('dotenv').config();

const apiKey = process.env.GAMESHIFT_KEY;

app.get('/v2/users/:email', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    let response = await axios.get(
      `https://api.gameshift.dev/v2/users/${refId}`,
      {
        headers: {
          accept: 'application/json',
          'x-api-key': apiKey,
        },
        validateStatus: function (status) {
          return (status >= 200 && status < 300) || status === 404;
        },
      },
    );

    if (response.status === 404) {
      response = await axios.post(
        'https://api.gameshift.dev/v2/users',
        {
          referenceId: refId,
          email: email,
        },
        {
          headers: {
            accept: 'application/json',
            'content-type': 'application/json',
            'x-api-key': apiKey,
          },
        },
      );

      const profileTemplate = '8bd9e0c7-eab5-4631-828a-639dbfd44167';

      response = await axios.post(
        'https://api.gameshift.dev/asset-templates/' +
          profileTemplate +
          '/assets',
        {
          destinationUserReferenceId: refId,
        },
        {
          headers: {
            accept: 'application/json',
            'content-type': 'application/json',
            'x-api-key': apiKey,
          },
        },
      );
      console.log('got the mint response', response);
    }

    const assetsResponse = await axios.get(
      `https://api.gameshift.dev/users/${refId}/assets`,
      {
        headers: {
          accept: 'application/json',
          'x-api-key': apiKey,
        },
      },
    );

    res.json({
      user: response.data,
      assets: assetsResponse.data,
    });
  } catch (error) {
    res
      .status(error.response ? error.response.status : 500)
      .send(error.response ? error.response.data : 'An error occurred');
  }
});

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`);
});
