import { Component, OnDestroy, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { Subscription } from "rxjs"
import { NotificationService } from "src/app/services/notification.service"
import { OrderService } from "src/app/services/order.service"

@Component({
	selector: "app-my-detail",
	templateUrl: "./my-detail.component.html",
	styleUrls: ["./my-detail.component.css"],
})
export class MyDetailComponent implements OnInit, OnDestroy {
	reloadSub: Subscription = null
	order = null
	detail = []

	constructor(private router: Router, private orderService: OrderService, private notif: NotificationService) {
		if (orderService.selectedOrder == null) {
			this.router.navigate(["/master/main"])
			return
		}
		this.order = orderService.selectedOrder
	}

	ngOnDestroy(): void {
		if (this.reloadSub != null) this.reloadSub.unsubscribe()
	}

	async onLoad() {
		await this.orderService.getDetail(false)
		this.detail = this.orderService.detail
		this.orderService.markViewed(this.order)
	}

	async ngOnInit(): Promise<void> {
		await this.onLoad()
		this.reloadSub = this.notif.onUpdateDetail.subscribe(async () => {
			if (this.orderService.selectedOrder == null) {
				this.router.navigate(["/master/main"])
				return
			}
			this.order = this.orderService.selectedOrder
			await this.onLoad()
		})
	}

	goToMain() {
		this.router.navigate(["/master/main"])
	}

	goBack() {
		this.router.navigate(["/master/myorders"])
	}
}
