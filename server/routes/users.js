var express = require('express');
var router = express.Router();
var axios = require('axios');
require('dotenv').config();

const apiKey = process.env.GAMESHIFT_KEY;
const apiRoot = process.env.GAMESHIFT_ROOT;
const profileTemplate = process.env.PROFILE_TEMPLATE;

router.get('/', function (req, res, next) {
  res.status(200).send('respond with a resource');
});

router.get('/:email', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    let response = await axios.get(`${apiRoot}/v2/users/${refId}`, {
      headers: {
        accept: 'application/json',
        'x-api-key': apiKey,
      },
      validateStatus: function (status) {
        return (status >= 200 && status < 300) || status === 404;
      },
    });

    if (response.status === 404) {
      response = await axios.post(
        `${apiRoot}/v2/users`,
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
        `${apiRoot}/asset-templates/${profileTemplate}/assets`,
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

    const assetsResponse = await axios.get(`${apiRoot}/users/${refId}/assets`, {
      headers: {
        accept: 'application/json',
        'x-api-key': apiKey,
      },
    });

    res.json({
      // user: response.data,
      assets: assetsResponse.data,
    });
  } catch (error) {
    res
      .status(error.response ? error.response.status : 500)
      .send(error.response ? error.response.data : 'An error occurred');
  }
});

router.put('/:email', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];
  console.log('inside');

  try {
    const assetsResponse = await axios.get(`${apiRoot}/users/${refId}/assets`, {
      headers: {
        accept: 'application/json',
        'x-api-key': apiKey,
      },
      validateStatus: function (status) {
        return (status >= 200 && status < 300) || status === 404;
      },
    });

    const profileNFT = assetsResponse.data.data.find(
      (nft) => nft.name === 'Profile NFT',
    );

    if (!profileNFT) {
      return res.status(404).send({ message: 'Profile NFT not found' });
    }

    const gamesPlayedAttribute = profileNFT.attributes.find(
      (attr) => attr.traitType === 'Games Played',
    );
    if (gamesPlayedAttribute) {
      const incrementedValue = parseInt(gamesPlayedAttribute.value, 10) + 1;

      const updatedAttributes = profileNFT.attributes.map((attr) => {
        if (attr.traitType === 'Games Played') {
          return {
            traitType: 'Games Played',
            value: incrementedValue.toString(),
          };
        }
        return attr;
      });

      await axios.put(
        `${apiRoot}/assets/${profileNFT.id}`,
        {
          imageUrl: profileNFT.imageUrl,
          attributes: updatedAttributes,
        },
        {
          headers: {
            accept: 'application/json',
            'content-type': 'application/json',
            'x-api-key': apiKey,
          },
        },
      );

      res.send({ message: 'Profile NFT updated successfully' });
    } else {
      res.status(400).send({ message: 'Games Played attribute not found' });
    }
  } catch (error) {
    console.error(
      'Error updating the NFT:',
      error.response ? error.response.data : error.message,
    );
    res
      .status(error.response ? error.response.status : 500)
      .send(
        error.response
          ? error.response.data
          : 'An error occurred while updating the NFT',
      );
  }
});

module.exports = router;
