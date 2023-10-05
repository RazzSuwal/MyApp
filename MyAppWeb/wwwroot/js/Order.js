var dtable;
$(document).ready(function () {

    var url = window.location.search;
    if (url.includes("pending")) {
        OrderTable("pending")
    }
    else (url.includes("approved"))
    {
        OrderTable("approved")
    }

    
});
function OrderTable(status) {
    dtable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/Order?status="+status

        },

        "columns": [
            { "data": "name" },
            { "data": "phone" },
            { "data": "city" },
            { "data": "orderStatus" },
            { "data": "orderTotal" },
            /*{ "data": "userId" },*/
            // {
            //     "data": "id",
            //     "render": function (data) {
            //         return `
            //                 <a href="/Admin/order/orderDetails?id=${data}"> Edit</a>
            //             `
            //     }
            // }
        ]
    });
}