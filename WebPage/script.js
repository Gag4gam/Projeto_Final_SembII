

function details_click(tytle){
    console.log(tytle);
    let id_element;
    let id_card

    switch (tytle){
        case 'accelerometer':
            id_element = "details_accelerometer"
            id_card = "accelerometer_card"
            break;
        case 'gyroscope':
            id_element = "details_gyroscope"
            id_card = "gyroscope_card"
            break;
        case 'magnetometer':
            id_element = "details_magnetometer"
            id_card = "magnetometer_card"
            break;
    }
    const element = document.getElementById(id_element);
    const card = document.getElementById(id_card);
    
    const isvisible = element.style.visibility === "visible";

    if (isvisible){
        element.style.visibility = "hidden";
        card.style.height = "50%";

    }else{
        element.style.visibility = "visible";
        card.style.width = "100%";
        card.style.height = "100%";
        
    }
}

function graphic_click(tytle){

    let id_element;

    console.log(tytle)
    switch (tytle){
        case 'accelerometer':
            id_element = "graphic_accelerometer"
            break;
        case 'gyroscope':
            id_element = "graphic_gyroscope"
            break;
        case 'magnetometer':
            id_element = "graphic_magnetometer"
            break;
    }
    console.log(id_element)
    const element = document.getElementById(id_element);

    const isvisible = element.style.visibility === "visible";

    if (isvisible){
        element.style.visibility = "hidden";


    }else{
        element.style.visibility = "visible";

        
    }

}