// Decrease Product Quantity
function decreaseQuantity(button) {
    const inputGroup = button.closest('.input-group');
    const hiddenInput = inputGroup.querySelector('input[type="hidden"][name="newQuantity"]');
    const min = parseInt(hiddenInput.getAttribute('data-min'));
    let value = parseInt(hiddenInput.value);

    if (!isNaN(value) && value > min) {
        hiddenInput.value = value - 1;
        hiddenInput.form.submit();
    }
}

// Increase Product Quantity
function increaseQuantity(button) {
    const inputGroup = button.closest('.input-group');
    const hiddenInput = inputGroup.querySelector('input[type="hidden"][name="newQuantity"]');
    const max = parseInt(hiddenInput.getAttribute('data-max'));
    let value = parseInt(hiddenInput.value);

    if (!isNaN(value) && value < max) {
        hiddenInput.value = value + 1;
        hiddenInput.form.submit();
    }
}

// Scroll To Product
document.addEventListener("DOMContentLoaded", function () {
    const urlParams = new URLSearchParams(window.location.search);
    const pid = urlParams.get("pid");

    if (pid) {
        const target = document.getElementById("product-" + pid);
        if (target) {
            target.scrollIntoView({ behavior: "smooth", block: "center" });
        }
    }
});