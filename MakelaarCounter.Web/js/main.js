function buildTable(data, tableId){
    data.forEach(element => {
        let row = document.createElement('tr');
        let nameCell = document.createElement('td');
        let amountCell = document.createElement('td');
        nameCell.innerHTML =  element.makelaar.name;
        amountCell.innerHTML = element.offerCount;    
        row.appendChild(nameCell);
        row.appendChild(amountCell);
        document.getElementById(tableId).appendChild(row);    
    });
}

function fetchMakelaarOfferCount(url, tableId){
    fetch(url).then(
        (response) => {
            response.json().then(
                data => {
                    buildTable(data, tableId);
                }
            );
        }    
    );
}

window.onload = () => {
    fetchMakelaarOfferCount('http://localhost:5000/makelaars/offer-count-amsterdam', 'table-body-amsterdam');
    fetchMakelaarOfferCount('http://localhost:5000/makelaars/offer-count-amsterdam-tuin', 'table-body-amsterdam-tuin');
}