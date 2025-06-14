document.addEventListener("DOMContentLoaded", function () {
    const toggleBtn = document.getElementById("darkModeToggle");
    const icon = document.getElementById("darkModeIcon");

    if (localStorage.getItem("darkMode") === "enabled") {
        document.body.classList.add("dark-mode");
        if (icon) {
            icon.classList.remove("fa-moon");
            icon.classList.add("fa-sun");
        }
    }

    if (toggleBtn) {
        toggleBtn.addEventListener("click", () => {
            document.body.classList.toggle("dark-mode");

            if (document.body.classList.contains("dark-mode")) {
                if (icon) {
                    icon.classList.remove("fa-moon");
                    icon.classList.add("fa-sun");
                }
                localStorage.setItem("darkMode", "enabled");
            } else {
                if (icon) {
                    icon.classList.remove("fa-sun");
                    icon.classList.add("fa-moon");
                }
                localStorage.setItem("darkMode", "disabled");
            }
        });
    }
});