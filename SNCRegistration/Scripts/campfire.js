var x = 100;
var y = 250;

function init() {
  setInterval(draw, 75);
}

function draw() {
  var canvas = document.getElementById('theCanvas');
  if (canvas.getContext) {
    // Canvas Supported
    var ctx = canvas.getContext('2d');

    ctx.clearRect(0, 0, 300, 300);

    ctx.fillStyle = "rgba(180,69,19, .95)";
    ctx.strokeStyle = "rgba(200,100,0, 0.75)";
    ctx.save();

    ctx.beginPath();
    ctx.moveTo(x + 10, y);
    ctx.lineTo(x + 10, y);
    ctx.lineTo(x + 100, y + 35);
    ctx.lineTo(x + 90, y + 50);
    ctx.lineTo(x, y + 15);
    ctx.closePath();
    ctx.fill();

    ctx.beginPath();
    ctx.moveTo(x + 90, y);
    ctx.lineTo(x + 90, y);
    ctx.lineTo(x + 100, y + 15);
    ctx.lineTo(x + 10, y + 50);
    ctx.lineTo(x, y + 35);
    ctx.closePath();
    ctx.fill();

    ctx.fillStyle = "rgba(255,200,0, 0.5)";
    var startY = y - (Math.floor(Math.random() * 20) + 20);

    ctx.beginPath();
    ctx.moveTo(x, startY);
    //        ctx.lineTo(x+25,y);
    //        ctx.lineTo(x,y-25);
    ctx.bezierCurveTo(x + 25, y, x + 50, y, x + 50, y - Math.floor(Math.random() * 50));
    //        ctx.lineTo(x+50, y);
    //        ctx.lineTo(x+50, y-50);
    ctx.bezierCurveTo(x + 50, y, x + 75, y, x + 100, y - (Math.floor(Math.random() * 20) + 20));
    //        ctx.lineTo(x+75, y)
    //        ctx.lineTo(x+100,y-25);
    ctx.quadraticCurveTo(x + 50, y + 75, x, startY);
    //        ctx.closePath();
    ctx.fill();
    //        ctx.stroke();

    var flamelety = y - (Math.floor(Math.random() * 25) + 50);
    ctx.beginPath();
    ctx.moveTo(x + 25, flamelety);
    ctx.bezierCurveTo(x - 50, y + 25, x + 100, y + 25, x + 25, flamelety);
    ctx.fill();

    flamelety = y - (Math.floor(Math.random() * 25) + 50);
    ctx.beginPath();
    ctx.moveTo(x + 75, flamelety);
    ctx.bezierCurveTo(x, y + 25, x + 150, y + 25, x + 75, flamelety);
    ctx.fill();

    flamelety = y - (Math.floor(Math.random() * 25) + 75);
    ctx.beginPath();
    ctx.moveTo(x + 50, flamelety);
    ctx.bezierCurveTo(x - 50, y + 25, x + 150, y + 25, x + 50, flamelety);
    ctx.fill();

    flamelety = y - (Math.floor(Math.random() * 25) + 50);
    ctx.beginPath();
    ctx.moveTo(x + 50, flamelety);
    ctx.bezierCurveTo(x - 50, y + 25, x + 150, y + 25, x + 50, flamelety);
    ctx.fill();

    console.log("Going");

  } else {
    // Canvas Unsupported 
  }
}

init();