import { Component, OnInit } from "@angular/core"
import { faPlus, faMinus } from "@fortawesome/free-solid-svg-icons"
import { AmmenityService } from "src/app/services/ammenity.service"
import { CartService } from "src/app/services/cart.service"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-ammenities",
	templateUrl: "./ammenities.component.html",
	styleUrls: ["./ammenities.component.css"],
})
export class AmmenitiesComponent implements OnInit {
	faPlus = faPlus
	faMinus = faMinus
	products = []
	cartMode = false

	constructor(private cart: CartService, private ammenity: AmmenityService, private notif: NotificationService) {}

	async ngOnInit(): Promise<void> {
		// this.analytics.log(`cart/15`)
		// this.cart.provider = 15
		await this.cart.loadProducts()
		const productArray = []
		for (const category of this.cart.categories) {
			for (const product of category.products) {
				productArray.push({
					id: product.id,
					name: product.name,
					description: product.description,
					image: product.image,
					quantity: 0,
				})
			}
		}
		this.products = productArray
	}

	increment(item) {
		if (item.quantity < 10) item.quantity++
	}

	decrement(item) {
		if (item.quantity > 0) item.quantity--
	}

	changeMode(value) {
		if (value && this.itemsCount > 0) this.cartMode = value
		else if (!value) this.cartMode = value
	}

	get productList() {
		if (this.cartMode) return this.products.filter(x => x.quantity > 0)
		else return this.products
	}

	get itemsCount() {
		return this.products.reduce((s, x) => s + x.quantity, 0)
	}

	goBack() {
		this.cartMode = false
	}

	resetCart() {
		this.products.forEach(x => (x.quantity = 0))
		this.cartMode = false
	}

	async confirmCart() {
		if (this.itemsCount <= 0) {
			this.notif.info("No hay productos en el carrito")
			return
		}
		const productsToSave = this.productList.map(p => ({
			id: p.id,
			quantity: p.quantity,
		}))
		await this.ammenity.saveOrder("", "", productsToSave, () => {
			this.resetCart()
		})
	}
}
