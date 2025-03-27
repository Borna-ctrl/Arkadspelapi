document.addEventListener('DOMContentLoaded', function () {
    const categoryFilter = document.getElementById('categoryFilter');
    const gameCards = document.querySelectorAll('.game-card');

    // Modal popup
    const modal = document.getElementById('gameModal');
    const closeBtn = document.querySelector('.close');
    const modalTitle = document.getElementById('modalTitle');
    const modalVideo = document.getElementById('modalVideo');
    const modalDescription = document.getElementById('modalDescription');

//Kategorin funkar inte :U
    categoryFilter.addEventListener('change', function () {
        const selectedCategory = categoryFilter.value;

        gameCards.forEach(function (card) {
            const cardCategory = card.getAttribute('data-category');

            if (selectedCategory === 'all' || selectedCategory === cardCategory) {
                card.style.display = 'block'; 
            } else {
                card.style.display = 'none'; 
            }
        });
    });

    // Funktion för att konvertera YouTube länk korrekt. Fick hjälp av chatgpt!
    function getEmbedUrl(url) {
        if (url.includes("youtube.com/watch?v=")) {
            const videoId = new URL(url).searchParams.get("v");
            return `https://www.youtube.com/embed/${videoId}?autoplay=1`;
        } else if (url.includes("youtu.be/")) {
            const videoId = url.split("youtu.be/")[1].split("?")[0]; 
            return `https://www.youtube.com/embed/${videoId}?autoplay=1`;
        }
        return url;
    }

    function openModal(trailerUrl, title, description) {
        if (trailerUrl.includes("watch?v=")) {
            trailerUrl = trailerUrl.replace("watch?v=", "embed/");
        }

        document.getElementById('modalTitle').innerText = title;
        document.getElementById('modalDescription').innerText = description;
        document.getElementById('modalVideo').src = trailerUrl;

        document.getElementById('gameModal').style.display = 'block';
    }

    document.querySelector('.close').addEventListener('click', function () {
        document.getElementById('gameModal').style.display = 'none';
        document.getElementById('modalVideo').src = ''; 
    });


    document.querySelectorAll('.game-card button').forEach(button => {
        button.addEventListener('click', function (event) {
            event.stopPropagation(); 

            const card = event.target.closest('.game-card'); 
            const title = card.querySelector('h3').innerText;
            const description = card.querySelector('p').innerText;
            const videoSrc = card.getAttribute('data-video');

            openModal(videoSrc, title, description);
        });
    });

    closeBtn.addEventListener('click', function () {
        modal.style.display = 'none';
        modalVideo.src = '';
    });

    window.addEventListener('click', function (event) {
        if (event.target === modal) {
            modal.style.display = 'none';
            modalVideo.src = '';
        }
    });
});

$(document).ready(function () {
    // Hämta alla spel från API:et 
    $.get("https://localhost:5232/api/spel", function (data) {
        var gameGrid = $(".game-grid");
        gameGrid.empty(); 

        // Gå igenom alla spel och lägg till varje spel som ett kort
        data.forEach(function (spel) {
            var gameCard = `
                <div class="game-card" data-category="${spel.Kategori}" data-video="${spel.TrailerUrl}">
                    <div class="badge">HOT 🔥</div>
                    <img src="${spel.BildUrl}" alt="${spel.Titel}">
                    <h3>${spel.Titel}</h3>
                    <p>${spel.Kategori}</p>
                    <button onclick="openModal('${spel.TrailerUrl}', '${spel.Titel}', 'Kategori: ${spel.Kategori}')">Se Trailer</button>
                </div>
            `;
            gameGrid.append(gameCard);
        });
    }).fail(function () {
        alert("Det gick inte att hämta spel från API.");
    });

    $("#createGame").submit(function (event) {
        event.preventDefault();

        const titel = $("#titel").val();
        const kategori = $("#kategori").val();
        const beskrivning = $("#beskrivning").val();
        const bildUrl = $("#bildUrl").val();
        const trailerUrl = $("#trailerUrl").val();

        const gameData = {
            Titel: titel,
            Kategori: kategori,
            Beskrivning: beskrivning,
            BildUrl: bildUrl,
            TrailerUrl: trailerUrl
        };

        $.ajax({
            url: "https://localhost:5232/api/spel",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(gameData),
            success: function (data) {
                alert("Spelet har skapats!");
                $.get("https://localhost:5232/api/spel", function (data) {
                    var gameGrid = $(".game-grid");
                    gameGrid.empty();
                    data.forEach(function (spel) {
                        var gameCard = `
                            <div class="game-card" data-category="${spel.Kategori}" data-video="${spel.TrailerUrl}">
                                <div class="badge">HOT 🔥</div>
                                <img src="${spel.BildUrl}" alt="${spel.Titel}">
                                <h3>${spel.Titel}</h3>
                                <p>${spel.Kategori}</p>
                                <button onclick="openModal('${spel.TrailerUrl}', '${spel.Titel}', 'Kategori: ${spel.Kategori}')">Se Trailer</button>
                            </div>
                        `;
                        gameGrid.append(gameCard);
                    });
                });

                window.location.href = "/Home/Index";
            },
            error: function () {
                alert("Det gick inte att skapa spelet. Försök igen.");
            }
        });
    });
});
