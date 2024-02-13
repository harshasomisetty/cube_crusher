const axios = require('axios');

const apiKey = process.env.GAMESHIFT_KEY;
const apiRoot = process.env.GAMESHIFT_ROOT;

const headers = {
  accept: 'application/json',
  'content-type': 'application/json',
  'x-api-key': apiKey,
};

const validateStatus = function (status) {
  return (status >= 200 && status < 300) || status === 404;
};

const getUserByRefId = async (refId) => {
  return await axios.get(`${apiRoot}/v2/users/${refId}`, {
    headers,
    validateStatus,
  });
};

const createUser = async (refId, email) => {
  return await axios.post(
    `${apiRoot}/v2/users`,
    { referenceId: refId, email },
    { headers },
  );
};

const postAssetTemplate = async (refId, profileTemplate) => {
  return await axios.post(
    `${apiRoot}/asset-templates/${profileTemplate}/assets`,
    { destinationUserReferenceId: refId },
    { headers },
  );
};

const getUserAssets = async (refId) => {
  return await axios.get(`${apiRoot}/users/${refId}/assets`, {
    headers: {
      accept: 'application/json',
      'x-api-key': apiKey,
    },
  });
};

const getUserProfileNFT = async (refId) => {
  const assetsResponse = await getUserAssets(refId);
  const profileNFT = assetsResponse.data.data.find(
    (nft) => nft.name === 'Profile NFT',
  );

  if (!profileNFT) {
    throw new Error('Profile NFT not found');
  }

  return profileNFT;
};

const updateAsset = async (assetId, imageUrl, attributes) => {
  return await axios.put(
    `${apiRoot}/assets/${assetId}`,
    { imageUrl, attributes },
    { headers },
  );
};

export {
  createUser,
  getUserAssets,
  getUserByRefId,
  getUserProfileNFT,
  postAssetTemplate,
  updateAsset,
};
