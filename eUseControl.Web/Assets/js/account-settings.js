// Trigger File Input on Button Click
document.querySelector('.btn-recycle').addEventListener('click', function (e) {
    e.preventDefault();
    document.getElementById('profileImage').click();
});

// Show File Selected Message
document.getElementById('profileImage').addEventListener('change', function () {
    if (this.files && this.files[0]) {
        document.getElementById('uploadSuccessMessage').classList.remove('d-none');
        document.getElementById('uploadSuccessMessage').innerText = '✅ ' + this.files[0].name + ' selected!';
    }
});