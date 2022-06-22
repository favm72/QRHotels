import { AfterViewChecked, Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { AccountService } from "src/app/services/account.service"
import { CartService } from "src/app/services/cart.service"
import { InfoService } from "src/app/services/info.service"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-cart",
	templateUrl: "./cart.component.html",
	styleUrls: ["./cart.component.css"],
})
export class CartComponent implements OnInit, AfterViewChecked {
	categories = []
	elements = 0
	showGotoCart = true
	hotel: any = {}
	constructor(
		private router: Router,
		private cart: CartService,
		private notif: NotificationService,
		private account: AccountService,
		private info: InfoService
	) {
		if (localStorage.qrh == null) {
			this.router.navigate(["/login"])
			return
		}
		if (cart.provider == 0) {
			this.router.navigate(["/master/digital"])
			return
		}
		this.showGotoCart = !cart.viewOnly
	}

	ngAfterViewChecked(): void {
		if (this.cart.product != null) {
			let el = document.querySelector(`section.producto[data-target="${this.cart.product.id}"]`)
			if (el != null) {
				el.scrollIntoView()
				window.scrollTo(window.scrollX, window.scrollY - 40)
				this.cart.product = null
			}
		}
	}

	onLogo() {
		this.router.navigate(["/master/main"])
	}

	goBack() {
		if (this.cart.provider == 1) this.router.navigate(["/master/main"])
		else this.router.navigate(["/master/digital"])
	}

	goToCart() {
		if (this.cart.order.length == 0) {
			this.notif.info("No ha agregado elementos al carrito")
			return
		}
		this.router.navigate(["/order"])
	}

	async getHotel() {
		this.hotel = await this.info.loadHotelInfo()
	}

	async ngOnInit(): Promise<void> {
		this.getHotel()
		await this.cart.loadProducts()
		this.cart.updateCart()
		this.categories = this.cart.categories
		this.elements = this.cart.elements
	}
}
