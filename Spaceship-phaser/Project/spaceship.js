var game = new Phaser.Game(800, 570, Phaser.CANVAS, 'spaceship', {
    preload: preload,
    create: create,
    update: update,
    // render: render,
});

var map;
var cursors;
var player;
var collisionLayer;
var terrainLayer;
var playerVelocity = 200;

function preload() {
    // debugger;

    game.load.tilemap('floor 1 csv', '/Spaceship/Assets/Tilemaps/floor 1 7 csv.json', null, Phaser.Tilemap.TILED_JSON);
    game.load.tilemap('floor 1', '/Spaceship/Assets/Tilemaps/floor 1 7.json', null, Phaser.Tilemap.TILED_JSON);

    game.load.image('border', '/Tiles/space station/layers/border-32.png');
    game.load.image('floor', '/Tiles/space station/layers/floor-32.png');
    game.load.image('light', '/Tiles/space station/layers/light-32.png');
    game.load.image('objects', '/Tiles/space station/layers/objects-32.png');
    game.load.image('shadows', '/Tiles/space station/layers/shadows-32.png');
    game.load.image('space', '/Tiles/space station/layers/space-32.png');
    game.load.image('upper body', '/Tiles/space station/layers/upper body-32.png');
    game.load.image('walls up', '/Tiles/space station/layers/walls up-32.png');
    game.load.image('walls vert', '/Tiles/space station/layers/walls vert-32.png');

    game.load.spritesheet('player', '/Spaceship/Assets/spaceman-32.png', 32, 32);
}

function create() {
    // scaleGame();
    initTilemap();
    initKeyboard();

    // game.input.keyboard.addKey(Phaser.KeyCode.C).onDown.add(() => {
    //     collisionLayer.visible = !collisionLayer.visible;
    // });
}

function update() {
    game.physics.arcade.collide(player, collisionLayer);

    player.body.velocity.set(0);

    if (cursors.left.isDown) {
        player.body.velocity.x = 0 - playerVelocity;
        player.play('left');
    } else if (cursors.right.isDown) {
        player.body.velocity.x = playerVelocity;
        player.play('right');
    } else if (cursors.up.isDown) {
        player.body.velocity.y = 0 - playerVelocity;
        player.play('up');
    } else if (cursors.down.isDown) {
        player.body.velocity.y = playerVelocity;
        player.play('down');
    } else {
        player.animations.stop();
    }
}

/**********************/

function scaleGame() {
    game.world.scale.x = 0.5;
    game.world.scale.y = 0.5;

    // game.scale.fullScreenScaleMode = Phaser.ScaleManager.SHOW_ALL;
    // game.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;
    // game.scale.refresh();

    // game.scale.scaleMode = Phaser.ScaleManager.RESIZE;
    // game.scale.pageAlignHorizontally = true;
    // game.scale.pageAlignVertically = true;
    // game.scale.refresh();
    // game.scale.setScreenSize(true);
}

function initTilemap() {
    // map = game.add.tilemap('floor 1');
    map = game.add.tilemap('floor 1 csv');
    // map.setTileSize(32, 32);

    addTilesetImages();

    // map.tilesets.map(function(ts) { ts.tileHeight = 32; ts.tileWidth = 32; });

    terrainLayer = map.createLayer('terrain');

    terrainLayer.scale.set(0.5);
    terrainLayer.resizeWorld();

    collisionLayer = map.createLayer('collision');
    collisionLayer.visible = false;
    map.setCollisionByExclusion([], true, collisionLayer);
    // collisionLayer.scale.set(20);
    // collisionLayer.resizeWorld();

    initPlayer();

    map.createLayer('shadows');
    map.createLayer('items');
}

function initPlayer() {
    player = game.add.sprite(48, 48, 'player', 1);
    player.animations.add('left', [8, 9], 10, true);
    player.animations.add('right', [1, 2], 10, true);
    player.animations.add('up', [11, 12, 13], 10, true);
    player.animations.add('down', [4, 5, 6], 10, true);

    game.physics.enable(player, Phaser.Physics.ARCADE);

    player.body.setSize(62, 62, 0, 0);
    // player.body.setSize(10, 14, 2, 1);

    game.camera.follow(player);
}

function addTilesetImages() {
    map.addTilesetImage('border', 'border');
    map.addTilesetImage('floor', 'floor');
    map.addTilesetImage('light', 'light');
    map.addTilesetImage('objects', 'objects');
    map.addTilesetImage('shadows', 'shadows');
    map.addTilesetImage('space', 'space');
    map.addTilesetImage('upper body', 'upper body');
    map.addTilesetImage('walls up', 'walls up');
    map.addTilesetImage('walls vert', 'walls vert');
}

function initKeyboard() {
    cursors = game.input.keyboard.createCursorKeys();
}
