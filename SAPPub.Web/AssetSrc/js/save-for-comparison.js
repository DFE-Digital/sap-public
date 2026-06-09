(function () {
    function init() {
        const forms = document.querySelectorAll(".save-establishment-comparison-form");

        if (!forms.length) {
            return;
        }

        forms.forEach(bindForm);
    }

    function bindForm(form) {
        form.addEventListener("submit", handleSubmit);
    }

    async function handleSubmit(event) {

        event.preventDefault();

        const form = event.currentTarget;

        try {
            const response = await fetch(
                form.action,
                {
                    method: "POST",
                    headers: { "X-Requested-With": "XMLHttpRequest" },
                    body: new FormData(form)
                });

            if (!response.ok) {
                throw new Error(`Request failed with status ${response.status}`);
            }

            const result = await response.json();

            updateButtonState(form, result.isSaved);

            toggleNotificationBanner(result.isLimitReached);

        }
        catch
        {
            // fallback to normal form submission
            form.submit();
        }
    }

    function updateButtonState(form, isSaved) {
        const saveText = form.dataset.saveText;
        const savedText = form.dataset.savedText;
        const button = form.querySelector("button");

        if (!button) {
            return;
        }

        button.innerHTML = `<span>${isSaved ? savedText : saveText}</span>`;
        button.classList.toggle("saved", isSaved);
    }

    function toggleNotificationBanner(showBanner) {
        const limitNotificationBanner = document.getElementById('comparision-limit-reached-banner');

        if (!limitNotificationBanner) {
            return;
        }

        if (showBanner) {
            limitNotificationBanner.classList.remove("govuk-!-display-none");
            limitNotificationBanner.focus();
            limitNotificationBanner.scrollIntoView({
                behavior: 'smooth'
            });
        }
        else {
            limitNotificationBanner.classList.add("govuk-!-display-none");
        }
    }

    document.addEventListener(
        "DOMContentLoaded",
        init);
})();