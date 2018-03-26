
app.controller('ProductController', ['$scope', 'HttpService', 'DTOptionsBuilder', 'DTColumnBuilder',
    function ($scope, HttpService, DTOptionsBuilder, DTColumnBuilder) {
        
        var vm = this;
        vm.message = '';
        vm.someClickHandler = someClickHandler;

        $scope.shoppingcar = [];

        $scope.dtColumns = [
            DTColumnBuilder.newColumn("idproduct", "ID Product").notVisible(),
            DTColumnBuilder.newColumn("product", "Product"),
            DTColumnBuilder.newColumn("description", "Description"),
            DTColumnBuilder.newColumn("sku", "SKU"),
            DTColumnBuilder.newColumn("price", "Price")
        ]

        $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
            url: "/Product/getProducts",
            type: "POST"
        })
            .withPaginationType('full_numbers')
            .withDisplayLength(10)
            .withOption('rowCallback', rowCallback);

        function someClickHandler(info) {
            $scope.GetProductByID(info);
            $("#productmodal").modal("show");
        }

        function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            // Unbind first in order to avoid any duplicate handler (see https://github.com/l-lin/angular-datatables/issues/87)
            $('td', nRow).unbind('click');
            $('td', nRow).bind('click', function () {
                $scope.$apply(function () {
                    vm.someClickHandler(aData);
                });
            });
            return nRow;
        }
            
        // Base Url 
        var baseUrl = '/api/Product/';
        $scope.btnText = "Save";
        $scope.productID = 0;
        $scope.categoryID = 0;
        $scope.SaveUpdate = function () {
            var product = {
                Product: $scope.product,
                IdCat: 1,
                Description: $scope.desc,
                Sku: $scope.sku,
                Price: $scope.price,
                Idproduct: $scope.productID
            }
            if ($scope.btnText == "Save") {
                var apiRoute = baseUrl + 'SaveProduct/';
                var saveProduct = HttpService.post(apiRoute, product);
                saveProduct.then(function (response) {
                    if (response.data != "") {
                        alert("Product Saved Successfully");
                        $scope.GetProducts();
                        $scope.Clear();

                    } else {
                        alert("Some error");
                    }

                }, function (error) {
                    console.log("Error: " + error);
                });
            }
            else {
                var apiRoute = baseUrl + 'UpdateProduct/';
                var UpdateProduct = HttpService.put(apiRoute, product);
                UpdateProduct.then(function (response) {
                    if (response.data != "") {
                        alert("Product Updated Successfully");
                        $scope.GetProducts();
                        $scope.Clear();

                    } else {
                        alert("Some error");
                    }

                }, function (error) {
                    console.log("Error: " + error);
                });
            }
        }


        $scope.GetProducts = function () {
            var apiRoute = baseUrl + 'GetProducts/';
            var product = HttpService.getAll(apiRoute);
            product.then(function (response) {
                debugger
                $scope.products = response.data;

            },
                function (error) {
                    console.log("Error: " + error);
                });


        }
        $scope.GetProducts();

        $scope.GetProductByID = function (dataModel) {
            debugger
            var apiRoute = baseUrl + 'GetProductByID';
            var product = HttpService.getbyID(apiRoute, dataModel.idproduct);
            product.then(function (response) {
                $scope.productID = response.data.idproduct;
                $scope.product = response.data.product;
                $scope.idcat = response.data.idcategory;
                $scope.desc = response.data.description;
                $scope.sku = response.data.sku;
                $scope.price = response.data.price;
                $scope.btnText = "Update";
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.PushSelectedProduct = function (idProduct) {
            var apiRoute = baseUrl + 'GetProductByID';
            var product = HttpService.getbyID(apiRoute, idProduct);
            product.then(function (response) {
                var product = {
                    Product: response.data.product,
                    IdCat: response.data.idcategory,
                    Description: response.data.description,
                    Sku: response.data.sku,
                    Price: response.data.price,
                    Idproduct: idProduct
                }
                $scope.shoppingcar.push(product);
                $("#productmodal").modal("hide");
            },
                function (error) {
                    console.log("Error: " + error);
                });
        }

        $scope.total = function () {
            var total = 0;
            for (product of $scope.shoppingcar) {
                debugger
                total += product.Price;
            }
            return total;
        }

        $scope.showSummary = function () {
            $("#purchasesummary").modal("show");
        }

        $scope.DeleteProduct = function (dataModel) {
            debugger
            var apiRoute = baseUrl + 'DeleteProduct/' + dataModel.idproduct;
            var deleteProduct = HttpService.delete(apiRoute);
            deleteProduct.then(function (response) {
                if (response.data != "") {
                    alert("Product Deleted Successfully");
                    $scope.GetProducts();
                    $scope.Clear();

                } else {
                    alert("Some error");
                }

            }, function (error) {
                console.log("Error: " + error);
            });
        }

        $scope.clearshoppingcar = function () {
            $scope.shoppingcar = [];
        }

        $scope.Clear = function () {
            $scope.product = "";
            $scope.idcat = 0;
            $scope.desc = "";
            $scope.sku = "";
            $scope.price = "";
            $scope.productID = 0;
            $scope.btnText = "Save";
        }

    }]);