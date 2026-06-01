(function () {
    document.addEventListener('submit', async function (e) {
        const form = e.target;

        if (!form.classList.contains("save-establishment-comparison-form")) {
            return;
        }

        e.preventDefault();

        const saveText = form.dataset.saveText;
        const savedText = form.dataset.savedText;
        const button = form.querySelector("button");
        const addSuccessBanner = form.querySelector("[id$='-add-success']");
        const removeSuccessBanner = form.querySelector("[id$='-remove-success']");
        const limitNotificationBanner = form.querySelector("[id$='-limit-notification']");

        if (button === null || addSuccessBanner === null || removeSuccessBanner === null) {
            return;
        }

        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: new FormData(form),
                headers: { "X-Requested-With": "XMLHttpRequest" }
            });

            if (!response.ok) {
                throw new Error(`Request failed with status ${response.status}`);
            }

            const data = await response.json();
            resetNotificationBanner(addSuccessBanner);
            resetNotificationBanner(removeSuccessBanner);

            button.classList.toggle("saved", data.saved);
            if (data.saved) {
                button.classList.add("saved");
                button.innerHTML = `<span>${savedText}</span>`;
                showNotificationBanner(addSuccessBanner);
            } else {
                button.classList.remove("saved");
                button.innerHTML = `<span>${saveText}</span>`;
                if (limitNotificationBanner !== null) {
                    limitNotificationBanner.classList.add("govuk-visually-hidden");
                }
                showNotificationBanner(removeSuccessBanner);
            }
        }
        catch {
            if (limitNotificationBanner !== null) {
                limitNotificationBanner.classList.remove("govuk-visually-hidden");
            }
        }
    });

    function resetNotificationBanner(bannerElement) {
        bannerElement.classList.add("govuk-visually-hidden");
    }

    function showNotificationBanner(bannerElement) {
        bannerElement.classList.remove("govuk-visually-hidden");
    }

})();