// script file

let tabledata = document.getElementById('tabledata')

fetch("https://savetocosmosdb.azurewebsites.net/api/GetAllFromCosmos?")
.then(res=> res.json())
.then(data=>{
    console.log(data)
    for(let row of data){
        tabledata.innerHTML += `
        <tr><td>${row.ID}</td>
        <td>${row.DeviceName}</td>
        <td>${row.Temperature}°C</td>
        <td>${row.Humidity}%</td>
        <td>${row.Brightness}%</td>
        <td>${(new Date(row.TimeSent * 1000)).toLocaleString()}</td>
        
        
        
        `
    }
    
})

 