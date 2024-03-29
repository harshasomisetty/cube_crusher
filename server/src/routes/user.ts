var express = require('express');
var router = express.Router();

require('dotenv').config();

import {
  awardToUser,
  checkUserExists,
  getUserAssets,
  getUserCharacters,
  getUserInventory,
  getUserProfileNFT,
  transferAsset,
  updateAsset,
} from '../services/user.service';

router.get('/', function (req, res, next) {
  res.status(200).send('user endpoint');
});

router.get('/:email', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    await checkUserExists(refId, email);

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

router.get('/:email/profile', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    await checkUserExists(refId, email);

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

router.get('/:email/inventory', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    await checkUserExists(refId, email);

    const inventory = await getUserInventory(refId);
    res.json({
      inventory,
    });
  } catch (error) {
    console.error('Error fetching inventory:', error.message);
    const statusCode = error.message === 'Inventory not found' ? 404 : 500;
    res.status(statusCode).send({
      message:
        error.message || 'An error occurred while fetching the inventory',
    });
  }
});

router.get('/:email/characters', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    await checkUserExists(refId, email);

    let characters = await getUserCharacters(refId);
    res.json({
      characters,
    });
  } catch (error) {
    res
      .status(error.response ? error.response.status : 500)
      .send(error.response ? error.response.data : 'An error occurred');
  }
});

router.get('/:email/gamesPlayed', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    await checkUserExists(refId, email);
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

router.post('/:email/award', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  try {
    const result = await awardToUser(refId);
    res.json({
      ...result,
    });
  } catch (error) {
    console.error('Error in /award route:', error.message);
    res.status(error.response?.status || 500).send({ message: error.message });
  }
});

router.post('/:email/transferAsset', async (req, res) => {
  const email = req.params.email;
  const refId = email.split('@')[0];

  const { assetId, destinationUserId, destinationWallet } = req.body;

  if (!assetId || (!destinationUserId && !destinationWallet)) {
    return res.status(400).send({ message: 'Missing required parameters' });
  }

  try {
    const destination = destinationUserId
      ? { destinationUserId }
      : { destinationWallet };

    const result = await transferAsset(assetId, refId, destination);

    res.json({ transferUrl: result.data });
  } catch (error) {
    console.error('Error in /transferAsset route:', error.message);
    res.status(error.response?.status || 500).send({ message: error.message });
  }
});

module.exports = router;
