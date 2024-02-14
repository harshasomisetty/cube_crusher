var express = require('express');
var axios = require('axios');
var router = express.Router();

router.get('/', function (req, res, next) {
  res.status(200).send('Cube Crusher API is running!');
});

router.get('/proxy-image', async (req, res) => {
  const imageUrl = req.query.url;
  if (!imageUrl) {
    return res.status(400).send('No URL provided');
  }

  try {
    const response = await axios({
      method: 'GET',
      url: imageUrl,
      responseType: 'arraybuffer',
      headers: {
        'Content-Type': 'image/jpeg',
      },
    });

    const contentType = response.headers['content-type'] || 'image/jpeg';
    res.set('Content-Type', contentType);

    const buffer = Buffer.from(response.data, 'binary');
    res.send(buffer);
  } catch (error) {
    console.error(error);
    res.status(500).send('Error fetching image');
  }
});

module.exports = router;
