import { coinDetails, starDetails } from '../bin/nftJson';

const axios = require('axios');

const apiKey = process.env.GAMESHIFT_KEY;
const apiRoot = process.env.GAMESHIFT_ROOT;
const profileTemplate = process.env.PROFILE_TEMPLATE;
const whiteTemplate = process.env.WHITE_TEMPLATE;
const redTemplate = process.env.RED_TEMPLATE;
const blueTemplate = process.env.BLUE_TEMPLATE;

const headers = {
  accept: 'application/json',
  'content-type': 'application/json',
  'x-api-key': apiKey,
};

export const validateStatus = function (status) {
  return (status >= 200 && status < 300) || status === 404;
};

export const getUserByRefId = async (refId) => {
  return await axios.get(`${apiRoot}/v2/users/${refId}`, {
    headers,
    validateStatus,
  });
};

export const createUser = async (refId, email) => {
  return await axios.post(
    `${apiRoot}/v2/users`,
    { referenceId: refId, email },
    { headers },
  );
};

export const postAssetTemplate = async (refId, templateId) => {
  return await axios.post(
    `${apiRoot}/asset-templates/${templateId}/assets`,
    { destinationUserReferenceId: refId },
    { headers },
  );
};

export const checkUserExists = async (refId, email) => {
  let response = await getUserByRefId(refId);

  if (response.status === 404) {
    await createUser(refId, email);
    await postAssetTemplate(refId, profileTemplate);
    await postAssetTemplate(refId, whiteTemplate);
  }
};

export const getUserAssets = async (refId) => {
  return await axios.get(`${apiRoot}/users/${refId}/assets`, {
    headers: {
      accept: 'application/json',
      'x-api-key': apiKey,
    },
  });
};

export const getUserCharacters = async (refId) => {
  let assetsResponse = await getUserAssets(refId);

  const colors = assetsResponse.data.data
    .filter((asset) => asset.name.endsWith('character'))
    .map((asset) => asset.name.split(' character')[0]);

  return colors;
};

export const getUserInventory = async (refId) => {
  let assetsResponse = await getUserAssets(refId);

  const inventoryItems = assetsResponse.data.data
    .filter(
      (asset) => asset.name !== 'Profile' && !asset.name.endsWith('character'),
    )
    .map((asset) => ({
      name: asset.name,
      description: asset.description,
      created: asset.created,
      attributes: asset.attributes,
      // environment: asset.environment,
      imageUrl: asset.imageUrl,
      mintAddress: asset.mintAddress,
      owner: asset.owner,
      priceCents: asset.priceCents,
    }));

  return inventoryItems;
};

export const getUserProfileNFT = async (refId) => {
  const assetsResponse = await getUserAssets(refId);
  const profileNFT = assetsResponse.data.data.find(
    (nft) => nft.name === 'Profile',
  );

  if (!profileNFT) {
    throw new Error('Profile NFT not found');
  }

  return profileNFT;
};

export const updateAsset = async (assetId, imageUrl, attributes) => {
  return await axios.put(
    `${apiRoot}/assets/${assetId}`,
    { imageUrl, attributes },
    { headers },
  );
};

export const awardToUser = async (refId) => {
  const isStarAward = Math.random() < 0.5;

  const details = isStarAward ? starDetails : coinDetails;

  try {
    const response = await axios.post(
      `${apiRoot}/assets`,
      {
        details,
        destinationUserReferenceId: refId,
      },
      { headers },
    );

    return {
      name: response.data.name,
      description: response.data.description,
      imageUrl: response.data.imageUrl,
    };
  } catch (error) {
    console.error('Error posting award to Gameshift:', error.message);
    throw error;
  }
};
