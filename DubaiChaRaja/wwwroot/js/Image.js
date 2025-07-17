function uploadImage() {
    const file = document.getElementById("fileInput").files[0];
    if (!file) return alert("Select a file first");

    const formData = new FormData();
    formData.append("file", file);

    $.ajax({
        url: "/Image/Upload",
        method: "POST",
        processData: false,
        contentType: false,
        data: formData
    }).done(() => {
        alert("Uploaded!");
        loadImages();
    });
}

let activeImageUrl = "";

function loadImages() {
    $.get("/Image/List", function (data) {
        const grid = $("#imageGrid").empty();
        data.forEach(image => {
            grid.append(`
                <div class="card shadow-sm border-0">
                    <div class="position-relative">
                        <img src="${image.url}"
                             class="card-img-top rounded w-100"
                             style="cursor:pointer; object-fit:cover; aspect-ratio: 4/3;"
                             onclick="openModal('${image.url}', '${image.date}', '${image.name}')" />
                    </div>
                    <div class="card-body p-2">
                        <p class="mb-0 small text-truncate" title="${image.name}">${image.name}</p>
                        <small class="text-muted">${new Date(image.date).toLocaleDateString()}</small>

                        <div class="d-flex justify-content-between mt-2">
                            <button class="btn btn-sm btn-outline-primary" onclick="downloadImage('${image.url}')">Download</button>
                            <button class="btn btn-sm btn-outline-danger" onclick="deleteImage('${image.url}')">Delete</button>
                        </div>
                    </div>
                </div>
            `);
        });
    });
}

function openModal(url, date, name) {
    activeImageUrl = url;
    $("#modalImage").attr("src", url);
    $("#modalMeta").html(`Name: ${name}<br>Date: ${new Date(date).toLocaleString()}`);
    $("#imageModal").modal("show");
}

function deleteImage(url) {
    if (!confirm("Are you sure?")) return;
    $.ajax({
        url: "/Image/Delete",
        type: "POST",
        data: JSON.stringify({ imageUrl: url }),
        contentType: "application/json"
    }).done(() => {
        $("#imageModal").modal("hide");
        loadImages();
    });
}

function downloadImage(imageurl) {
    const a = document.createElement("a");
    a.href = imageurl;
    a.download = imageurl.split("/").pop();
    a.click();
}

$(document).ready(loadImages);