(function () {
    const host = document.getElementById('map');
    if (!host) return;

    // ============================================================
    // CONFIG — DEMO LOCATION: THE LESCAR (SHARROW VALE RD)
    // ============================================================
    const DEMO_LOCATION = { lat: 53.367333, lon: -1.498417 }; // The Lescar, S11 8ZF

    // Balanced 360° set of primary schools within 3 miles (straight-line)
    const HARDCODED_SCHOOLS = [
        { name: "Broomhill Infant School", lat: 53.37930, lon: -1.50140 }, // N/NE
        { name: "St Marie’s School", lat: 53.37950, lon: -1.51130 }, // N/NW
        { name: "Westways Primary School", lat: 53.38340, lon: -1.50530 }, // NW/W
        { name: "Hallam Primary School", lat: 53.38360, lon: -1.53380 }, // W/NW
        { name: "Sharrow Primary School", lat: 53.36660, lon: -1.47430 }, // E/SE
        { name: "Porter Croft Primary Academy", lat: 53.36870, lon: -1.47930 }, // E/SE
        { name: "Greystones Primary School", lat: 53.36020, lon: -1.50000 }, // S/SW
        { name: "Ecclesall Primary School", lat: 53.34590, lon: -1.51300 }, // S/SW
        { name: "Nether Edge Primary School", lat: 53.35750, lon: -1.49110 }, // S/SE
        { name: "Holt House Infant School", lat: 53.34200, lon: -1.49350 }  // S/SE
    ];

    const radiusMiles = parseFloat(host.dataset.radiusMiles || "3");
    const fixedZoom = parseInt(host.dataset.fixedZoom || "14", 10);
    const RADIUS_METERS = radiusMiles * 1609.344;

    // ============================================================
    // ICONS
    // ============================================================
    const userIcon = L.icon({
        iconUrl: "/assets/images/markers/marker-user.svg",
        iconSize: [36, 54], iconAnchor: [18, 52], popupAnchor: [0, -44],
    });
    const targetIcon = L.icon({
        iconUrl: "/assets/images/markers/marker-school-target.svg",
        iconSize: [36, 54], iconAnchor: [18, 52], popupAnchor: [0, -44],
    });
    const nearbyIcon = L.icon({
        iconUrl: "/assets/images/markers/marker-school-nearby.svg",
        iconSize: [36, 54], iconAnchor: [18, 52], popupAnchor: [0, -44],
    });

    // ============================================================
    // ZOOM HINT
    // ============================================================
    function showZoomHint(mapContainer) {
        let hint = mapContainer.querySelector(".map-zoom-hint");
        if (!hint) {
            hint = document.createElement("div");
            hint.className = "map-zoom-hint";
            hint.textContent = "Use Ctrl + scroll to zoom the map";
            mapContainer.appendChild(hint);
        }
        hint.classList.add("visible");
        clearTimeout(hint._hideTimer);
        hint._hideTimer = setTimeout(() => hint.classList.remove("visible"), 3000);
    }

    // ============================================================
    // ACCESSIBILITY (TAB NAVIGATION)
    // ============================================================
    const focusableMarkerEls = [];

    function a11yMarker(marker, label) {
        marker.on("add", () => {
            const el = marker.getElement?.() || marker._icon;
            if (!el) return;

            el.setAttribute("role", "button");
            el.setAttribute("tabindex", "0");
            el.setAttribute("aria-label", label);
            el.classList.add("marker-focus");

            focusableMarkerEls.push(el);

            el.addEventListener("keydown", (e) => {
                if (e.key === "Enter" || e.key === " ") {
                    e.preventDefault();
                    marker.openPopup();
                }
            });
        });
        return marker;
    }

    host.addEventListener("keydown", (e) => {
        if (!["ArrowLeft", "ArrowRight"].includes(e.key)) return;

        const active = document.activeElement;
        if (!host.contains(active)) return;

        const idx = focusableMarkerEls.indexOf(active);
        const count = focusableMarkerEls.length;
        if (count === 0) return;

        const nextIdx =
            e.key === "ArrowRight"
                ? (idx + 1) % count
                : (idx - 1 + count) % count;

        focusableMarkerEls[nextIdx].focus();
    });

    // ============================================================
    // FIXED DEMO LOCATION (NO REAL GEOLOCATION)
    // ============================================================
    function getUserLocation() {
        return Promise.resolve({
            lat: DEMO_LOCATION.lat,
            lon: DEMO_LOCATION.lon,
            acc: 40,
        });
    }

    // ============================================================
    // PROXIMITY-BASED DE-OVERLAP
    // ============================================================
    const MIN_SEPARATION_METERS = 25;
    const overlapGroups = [];

    function registerMarker(marker, ll) {
        const latlng = L.latLng(ll);

        let group = overlapGroups.find(
            (g) => g.center.distanceTo(latlng) <= MIN_SEPARATION_METERS
        );

        if (!group) {
            group = { center: latlng, markers: [] };
            overlapGroups.push(group);
        }

        group.markers.push({ marker, latlng });
    }

    function applyOffsets(map) {
        overlapGroups.forEach((group) => {
            const n = group.markers.length;

            if (n <= 1) {
                group.markers[0].marker.setLatLng(group.markers[0].latlng);
                return;
            }

            const centerPt = map.latLngToLayerPoint(group.center);
            const radiusPx = 18;

            group.markers.forEach((entry, i) => {
                const angle = (2 * Math.PI * i) / n;
                const offsetPt = L.point(
                    centerPt.x + radiusPx * Math.cos(angle),
                    centerPt.y + radiusPx * Math.sin(angle)
                );
                entry.marker.setLatLng(map.layerPointToLatLng(offsetPt));
            });
        });
    }

    // ============================================================
    // MAIN
    // ============================================================
    Promise.all([
        Promise.resolve(HARDCODED_SCHOOLS),
        getUserLocation(),
    ]).then(([schools, user]) => {
        const userLL = L.latLng(user.lat, user.lon);

        const schoolData = schools.map((s) => ({
            name: s.name,
            ll: L.latLng(s.lat, s.lon),
            distance: L.latLng(s.lat, s.lon).distanceTo(userLL),
        }));

        const closest = schoolData.reduce((a, b) =>
            a.distance < b.distance ? a : b
        );

        const loading = host.querySelector(".map-loading");
        if (loading) loading.remove();

        const map = L.map(host, { scrollWheelZoom: true }).setView(
            closest.ll,
            fixedZoom
        );

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            maxZoom: 19,
            attribution: "© OpenStreetMap contributors",
        }).addTo(map);

        setTimeout(() => map.invalidateSize(), 0);

        const layer = L.featureGroup().addTo(map);

        // Scroll behaviour
        let zoomHintShown = false;
        map.getContainer().addEventListener(
            "wheel",
            (e) => {
                if (!e.ctrlKey && !e.metaKey) {
                    e.preventDefault();
                    if (!zoomHintShown) showZoomHint(host);
                    zoomHintShown = true;
                }
            },
            { passive: false }
        );

        map.on("mouseover", () => map.scrollWheelZoom.enable());
        map.on("mouseout", () => map.scrollWheelZoom.disable());

        // ============================================================
        // DEMO LOCATION MARKER (THE LESCAR)
        // ============================================================
        const um = L.marker(userLL, { icon: userIcon })
            .bindPopup("Demo location: The Lescar, Sharrow Vale Rd")
            .addTo(layer);

        a11yMarker(um, "Demo location: The Lescar pub");
        registerMarker(um, userLL);
        //L.circle(userLL, { radius: user.acc }).addTo(layer);

        // ============================================================
        // CLOSEST SCHOOL
        // ============================================================
        const cm = L.marker(closest.ll, { icon: targetIcon })
            .bindPopup(
                `<strong>${closest.name}</strong><br>${(
                    closest.distance / 1609.344
                ).toFixed(2)} miles away`
            )
            .addTo(layer);

        a11yMarker(cm, `${closest.name}, closest school`);
        registerMarker(cm, closest.ll);

        // ============================================================
        // OTHER SCHOOLS (WITHIN 3 MILES)
        // ============================================================
        schoolData
            .filter((s) => s !== closest && s.distance <= RADIUS_METERS)
            .forEach((s) => {
                //const marker = L.marker(s.ll, { icon: nearbyIcon })
                //    .bindPopup(
                //        `<strong>${s.name}</strong><br>${(
                //            s.distance / 1609.344
                //        ).toFixed(2)} miles away`
                //    )
                //    .addTo(layer);

                //a11yMarker(marker, `${s.name}, primary school`);
                //registerMarker(marker, s.ll);
            });

        // ============================================================
        // AUTO-FIT — MAX ZOOM 14, NEVER OVER-ZOOMED
        // ============================================================
        const bounds = layer.getBounds();
        const maxZoomFit = 19;

        if (bounds.isValid()) {
            map.fitBounds(bounds.pad(0.10), {
                padding: [40, 40],
                maxZoom: maxZoomFit,
                animate: false,
            });

            // prevent zooming way out if markers are sparse
            //if (map.getZoom() < fixedZoom) {
            //    map.setZoom(fixedZoom);
            //}
        }

        // ============================================================
        // APPLY DE-OVERLAP
        // ============================================================
        applyOffsets(map);
        map.on("zoomend", () => applyOffsets(map));
    });
})();
