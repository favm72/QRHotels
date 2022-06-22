import { Component, OnInit } from "@angular/core"
import { OrderService } from "src/app/services/order.service"

@Component({
	selector: "app-admin-products",
	templateUrl: "./admin-products.component.html",
	styleUrls: ["./admin-products.component.css"],
})
export class AdminProductsComponent implements OnInit {
	providers = []

	constructor(private order: OrderService) {}

	async ngOnInit(): Promise<void> {
		this.providers = await this.order.getProducts()
	}

	async toggle(product) {
		await this.order.toggleProduct(product.id)
	}
}
