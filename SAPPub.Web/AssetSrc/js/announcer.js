(function () {
    const region = document.createElement('div');
    region.className = 'govuk-visually-hidden';
    region.setAttribute('role', 'status');
    region.setAttribute('aria-live', 'polite');
    region.setAttribute('aria-atomic', 'true');
    document.body.appendChild(region);

    window.sapAnnounce = function (message) {
        region.textContent = message;
    };

})();