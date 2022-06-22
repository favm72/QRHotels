import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { AnalyticsService } from "src/app/services/analytics.service"
import { CartService } from "src/app/services/cart.service"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-services",
	templateUrl: "./services.component.html",
	styleUrls: ["./services.component.css"],
})
export class ServicesComponent implements OnInit {
	constructor(private router: Router, private analytics: AnalyticsService) {}

	ngOnInit(): void {}

	async goToDirectory() {
		await this.analytics.log("directory")
		this.router.navigate(["/master/directory"])
	}

	goToBiosecurity() {
		this.analytics.log(`Biosecurity`)
		window.open("https://www.casa-andina.com/es/protocolo-de-salubridad", "_blank")
	}
}
