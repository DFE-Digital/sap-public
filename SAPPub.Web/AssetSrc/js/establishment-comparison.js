(function () {
    document.addEventListener('submit', async function (e) {
        const form = e.target;
        const establishmentComparisonConfig = window.establishmentComparisonConfig;

        if (!form.classList.contains("save-establishment-comparison-form")) {
            return;
        }

        e.preventDefault();

        const response = await fetch(form.action, {
            method: "POST",
            body: new FormData(form),
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        const data = await response.json();
        const button = form.querySelector("button");
        const addSuccessBanner = document.getElementById("comparison-add-success");
        const removeSuccessBanner = document.getElementById("comparison-remove-success");
        resetNotificationBanner(addSuccessBanner);
        resetNotificationBanner(removeSuccessBanner);

        button.classList.toggle("saved", data.saved);
        if (data.saved) {
            button.classList.add("saved");
            button.innerHTML = `<span>${establishmentComparisonConfig.savedText}</span>`;
            showNotificationBanner(addSuccessBanner);
        } else {
            button.classList.remove("saved");
            button.innerHTML = `<span>${establishmentComparisonConfig.saveText}</span>`;
            const limitNotificationBanner = document.getElementById("school-compare-limit-notification");
            if (limitNotificationBanner !== null) {
                limitNotificationBanner.classList.add("govuk-visually-hidden");
            }
            showNotificationBanner(removeSuccessBanner);
        }
    });

    function resetNotificationBanner(bannerElement) {
        bannerElement.classList.add("govuk-visually-hidden");
    }

    function showNotificationBanner(bannerElement) {
        bannerElement.classList.remove("govuk-visually-hidden");
    }

})();