var express = require('express');
var router = express.Router();
require('dotenv').config();
import {
  createUser,
  getUserAssets,
  getUserByRefId,
  getUserProfileNFT,
  postAssetTemplate,
  updateAsset,
} from '../services/user.service';

const profileTemplate = process.env.PROFILE_TEMPLATE;

router.get('/', function (req, res, next) {
  res.status(200).send('user endpoint');
});

// Fetch all user assets
router.get('/:email', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    let response = await getUserByRefId(refId);

    if (response.status === 404) {
      await createUser(refId, email);
      await postAssetTemplate(refId, profileTemplate);
    }

    const assetsResponse = await getUserAssets(refId);

    res.json({
      assets: assetsResponse.data,
    });
  } catch (error) {
    res
      .status(error.response ? error.response.status : 500)
      .send(error.response ? error.response.data : 'An error occurred');
  }
});

// get user profile NFT
router.get('/:email/profile', async (req, res) => {
  console.log('get user profile NFT');
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    const profileNFT = await getUserProfileNFT(refId);
    res.json({
      profileNFT,
    });
  } catch (error) {
    console.error('Error fetching user profile NFT:', error.message);
    const statusCode = error.message === 'Profile NFT not found' ? 404 : 500;
    res.status(statusCode).send({
      message: error.message || 'An error occurred while fetching the profile',
    });
  }
});

router.get('/:email/profile/gamesPlayed', async (req, res) => {
  console.log('get user profile NFT');
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    const profileNFT = await getUserProfileNFT(refId);
    res.json({
      email: email,
      gamesPlayed: profileNFT.attributes.find(
        (attr) => attr.traitType === 'Games Played',
      ).value,
    });
  } catch (error) {
    console.error('Error fetching Games Played', error.message);
    const statusCode = error.message === 'Profile NFT not found' ? 404 : 500;
    res.status(statusCode).send({
      message: error.message || 'An error occurred while fetching the profile',
    });
  }
});

// Finished game on client, updating profile NFT with games played
router.put('/:email/finishGame', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    const profileNFT = await getUserProfileNFT(refId);

    const gamesPlayedAttribute = profileNFT.attributes.find(
      (attr) => attr.traitType === 'Games Played',
    );
    if (!gamesPlayedAttribute) {
      return res
        .status(400)
        .send({ message: 'Games Played attribute not found' });
    }

    const incrementedValue = parseInt(gamesPlayedAttribute.value, 10) + 1;
    const updatedAttributes = profileNFT.attributes.map((attr) =>
      attr.traitType === 'Games Played'
        ? { ...attr, value: incrementedValue.toString() }
        : attr,
    );

    await updateAsset(profileNFT.id, profileNFT.imageUrl, updatedAttributes);
    res.send({ message: 'Profile NFT updated successfully' });
  } catch (error) {
    console.error('Error:', error.message);
    res
      .status(error.message === 'Profile NFT not found' ? 404 : 500)
      .send({ message: error.message });
  }
});

module.exports = router;
