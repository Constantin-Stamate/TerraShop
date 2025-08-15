document.addEventListener("DOMContentLoaded", function () {

    // Heart Pulse Animation
    const hearts = document.querySelectorAll('.pulsing-heart');
    hearts.forEach((heart) => {
        let scaleUp = true;
        setInterval(() => {
            heart.style.transform = scaleUp ? 'scale(1.3)' : 'scale(1)';
            scaleUp = !scaleUp;
        }, 500);
    });

    // Button Pulse Animation
    const buttons = document.querySelectorAll(".pulse-js");
    buttons.forEach(button => {
        setInterval(() => {
            button.style.transform = "scale(1.05)";
            setTimeout(() => {
                button.style.transform = "scale(1)";
            }, 300);
        }, 1000);
    });

    // Expiration Date Input Toggle
    const input = document.getElementById('expDate');
    if (input) {
        input.addEventListener('focus', () => {
            input.type = 'date';
        });
        input.addEventListener('blur', () => {
            if (!input.value) {
                input.type = 'text';
            }
        });
    }

    // Star Rating Click Effect
    const stars = document.querySelectorAll('.star-rating i');
    stars.forEach(star => {
        star.addEventListener('click', () => {
            const index = parseInt(star.getAttribute('data-index'));
            stars.forEach((s, i) => {
                if (i < index) {
                    s.classList.add('text-warning');
                    s.classList.remove('text-muted');
                } else {
                    s.classList.add('text-muted');
                    s.classList.remove('text-warning');
                }
            });
        });
    });

    // Star Rating with Input Binding
    const ratingInput = document.getElementById("RatingInput");
    const starsRating = document.querySelectorAll(".star-rating .fa-star");
    if (ratingInput && starsRating.length) {
        const ratingValue = parseInt(ratingInput.value);

        starsRating.forEach((star, index) => {
            if (index < ratingValue) {
                star.classList.remove("text-muted");
                star.classList.add("text-secondary");
            } else {
                star.classList.remove("text-secondary");
                star.classList.add("text-muted");
            }
        });

        starsRating.forEach((star, index) => {
            star.addEventListener("click", () => {
                ratingInput.value = index + 1;

                starsRating.forEach((s, i) => {
                    if (i <= index) {
                        s.classList.remove("text-muted");
                        s.classList.add("text-secondary");
                    } else {
                        s.classList.remove("text-secondary");
                        s.classList.add("text-muted");
                    }
                });
            });
        });
    }
});