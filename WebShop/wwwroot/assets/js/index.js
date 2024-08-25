
/*scroll Header*/
 window.addEventListener('scroll', function() {
    var header = document.querySelector('.header');
    var scrollPosition = window.scrollY;

    // Thêm hoặc xóa lớp dựa trên vị trí cuộn
    if (scrollPosition > 0) {
        header.classList.add('scrolled');
    } else {
        header.classList.remove('scrolled');
    }
});

document.addEventListener("DOMContentLoaded", function () {
var scrollToTopButton = document.querySelector('.scroll-to-top');
scrollToTopButton.addEventListener('click', function () {
window.scrollTo({
  top: 0,
  behavior: 'smooth' // Tạo hiệu ứng cuộn mượt
  });
});

window.addEventListener('scroll', function() {
var scrollToTopButton = document.querySelector('.scroll-to-top');
var scrollPosition = window.scrollY;

if (scrollPosition > 100) {
scrollToTopButton.style.display = 'block';
} else {
  scrollToTopButton.style.display = 'none';
  }
});
document.addEventListener("DOMContentLoaded", function() {
var scrollToTopButton = document.querySelector('.scroll-to-top');

scrollToTopButton.addEventListener('click', function() {
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  });
  });
});   
});

/////
window.addEventListener('scroll', function() {
var header = document.querySelector('.header');
var scrollPosition = window.scrollY;

// Add or remove the 'scrolled' class based on the scroll position
if (scrollPosition > 0) {
header.classList.add('scrolled');
} else {
header.classList.remove('scrolled');
}
});


////time
document.addEventListener("DOMContentLoaded", function () {
// Call the function to initialize countdown
initializeCountdown();

// Function to initialize countdown
function initializeCountdown() {
// Set the date we're counting down to (current time + 2 days)
var countDownDate = new Date();
countDownDate.setDate(countDownDate.getDate() + 2);
countDownDate.setHours(0, 0, 0, 0);

countDownDate = countDownDate.getTime();

// Update the countdown every 1 second
var x = setInterval(function () {
    // Get the current date and time
    var now = new Date().getTime();

    // Calculate the remaining time
    var distance = countDownDate - now;

    // Calculate days, hours, minutes, and seconds
    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

    // Display the countdown in the corresponding div elements
    document.getElementById("days").innerHTML = days;
    document.getElementById("hours").innerHTML = hours;
    document.getElementById("minutes").innerHTML = minutes;
    document.getElementById("seconds").innerHTML = seconds;

    // If the countdown is over, display a message
    if (distance < 0) {
        clearInterval(x);
        document.getElementById("deal-time").innerHTML = "EXPIRED";
    }
}, 1000);
}
});


document.addEventListener('DOMContentLoaded', function() {
var togglerButton = document.querySelector('.navbar-toggler');
var navbarNav = document.querySelector('#navbarNav');

togglerButton.addEventListener('click', function() {
navbarNav.classList.toggle('show');
});
});


