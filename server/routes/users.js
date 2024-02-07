var express = require('express');
var router = express.Router();
var axios = require('axios');
require('dotenv').config();

const apiKey = process.env.GAMESHIFT_KEY;
const profileTemplate = process.env.PROFILE_TEMPLATE;

router.get('/', function (req, res, next) {
  res.status(200).send('respond with a resource');
});

router.get('/:email', async (req, res) => {
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

      response = await axios.post(
        `https://api.gameshift.dev/asset-templates/${profileTemplate}/assets`,
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

module.exports = router;
